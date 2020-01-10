namespace Sport.Services.Implementation
{
    using Data;
    using Domain;
    using ViewModels.Match;

    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;

    public class MatchService : IMatchService
    {
        private readonly SportDbContext context;
        private readonly IMapper mapper;


        public MatchService(SportDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Match>> GetAllActive()
        {
            var matches = await this.context
                .Matches
                .Where(m => m.IsActive)
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .ToListAsync();

            return matches;
        }

        public MatchScoreViewModel GetMatch(int id)
        {
            var dbMatch = this.context
                .Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .FirstOrDefault(m => m.Id == id);

            var match = mapper.Map<MatchScoreViewModel>(dbMatch);

            return match;
        }

        public async Task<UmpireResultViewModel> Result(string buttonId, int matchId)
        {
            Set set = null;
            Game game = null;
            Point point = null;
            TieBreakPoint tieBreakPoint = null;
            TieBreak tiebreak = null;

            bool IsFirstPlayer = buttonId.Equals("firstButtonId");
            bool IsSecondPlayer = buttonId.Equals("secondButtonId");


            var match = await this.context
            .Matches
            .Include(m => m.FirstPlayer)
            .Include(m => m.SecondPlayer)
            .Include(m => m.Umpire)
            .Include(m => m.Sets)
            .ThenInclude(a => a.Games)
            .ThenInclude(p => p.Points)
            .FirstOrDefaultAsync(m => m.Id == matchId);


            set = match.Sets.ToList().LastOrDefault();

            tiebreak = await this.context
                .TieBreaks
                .Include(t => t.TieBreakPoints)
                .FirstOrDefaultAsync(t => t.SetId == set.Id);


            if (set.IsSetFinished)
            {
                set = new Set()
                {
                    Games = new List<Game>()
                    {
                        new Game()
                        {
                            Points = new List<Point>()
                            {
                                new Point()
                                {
                                    FirstPlayerPoints=0,
                                    SecondPlayerPoints=0
                                }
                            }
                        }
                    }
                };

                match.Sets.Add(set);
            }

            game = set.Games.LastOrDefault();

            if ((game == null || game.IsGameFinished) && !set.HasTieBreak)
            {
                game = new Game()
                {
                    Set = set,
                    SetId = set.Id,
                    Points = new List<Point>()
                    {
                        new Point()
                    }
                };

                set.Games.Add(game);
            }

            if (!set.HasTieBreak)
            {
                var lastPoint = game.Points.ToList().LastOrDefault();

                point = new Point()
                {
                    FirstPlayerPoints = lastPoint.FirstPlayerPoints,
                    SecondPlayerPoints = lastPoint.SecondPlayerPoints,
                    GameId = lastPoint.GameId,
                    Game = lastPoint.Game
                };
            }
            else
            {
                var lastTieBreakPoint = tiebreak.TieBreakPoints.ToList().LastOrDefault(p => p.TieBreakId == tiebreak.Id);
                if (lastTieBreakPoint != null)
                {
                    tieBreakPoint = new TieBreakPoint()
                    {
                        TieBreak = tiebreak,
                        TieBreakId = tiebreak.Id,
                        FirstPlayerPoint = lastTieBreakPoint.FirstPlayerPoint,
                        SecondPlayerpoint = lastTieBreakPoint.SecondPlayerpoint
                    };
                }

            }

            if (IsFirstPlayer)
            {
                if (!set.HasTieBreak)
                {
                    point.FirstPlayerPoints++;
                }
                else
                {
                    tieBreakPoint.FirstPlayerPoint++;
                }

            }

            if (IsSecondPlayer)
            {
                if (!set.HasTieBreak)
                {
                    point.SecondPlayerPoints++;
                }
                else
                {
                    tieBreakPoint.SecondPlayerpoint++;
                }
            }

            if (!match.IsFinished)
            {
                if (!set.HasTieBreak)
                {

                    //if (((point.FirstPlayerPoints - 2 >= point.SecondPlayerPoints) && point.SecondPlayerPoints >= 3)
                    //|| (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints <= 2))

                    if (IsFirstPlayer)
                    {
                        if (((point.FirstPlayerPoints - 2 >= point.SecondPlayerPoints) && point.SecondPlayerPoints >= 3)
                    || (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints <= 2))
                        {
                            point.FirstPlayerPoints--;
                            game.Points.Add(point);
                            point.FirstPlayerPoints = 0;
                            point.SecondPlayerPoints = 0;
                            game.Player = match.FirstPlayer;
                            game.PlayerId = match.FirstPlayerId;
                            game.IsGameFinished = true;
                            set.FirstPlayerGames++;
                            set.Games.Add(game);


                            if ((set.FirstPlayerGames >= 6 && set.SecondPlayerGames <= 4)
                                || set.FirstPlayerGames == 7 && set.SecondPlayerGames == 5)
                            {
                                match.FirstPlayerSets++;
                                set.Player = match.FirstPlayer;
                                set.PlayerId = match.FirstPlayerId;
                                set.IsSetFinished = true;

                                if (match.FirstPlayerSets == 2)
                                {
                                    match.IsFinished = true;
                                    match.IsActive = false;

                                    //TODO Add Winner
                                }

                            }
                            else if (set.FirstPlayerGames == 6 && set.SecondPlayerGames == 6)
                            {
                                set.HasTieBreak = true;

                                TieBreak tieBreak = new TieBreak
                                {
                                    Set = set,
                                    SetId = set.Id,
                                    TieBreakPoints = new List<TieBreakPoint>()
                                };

                                tieBreak.TieBreakPoints.Add(tieBreakPoint);

                            }

                        }
                        else if (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints == 4)
                        {
                            point.FirstPlayerPoints = 3;
                            point.SecondPlayerPoints = 3;
                        }

                        game.Points.Add(point);

                    }

                    if (IsSecondPlayer)
                    {
                        if (((point.SecondPlayerPoints - 2 >= point.FirstPlayerPoints) && point.FirstPlayerPoints >= 3)
                    || (point.SecondPlayerPoints == 4 && point.FirstPlayerPoints <= 2))
                        {
                            point.SecondPlayerPoints--;
                            game.Points.Add(point);
                            point.FirstPlayerPoints = 0;
                            point.SecondPlayerPoints = 0;
                            game.Player = match.SecondPlayer;
                            game.PlayerId = match.SecondPlayerId;
                            game.IsGameFinished = true;
                            set.SecondPlayerGames++;


                            if ((set.SecondPlayerGames >= 6 && set.FirstPlayerGames <= 4)
                                || set.SecondPlayerGames == 7 && set.FirstPlayerGames == 5)
                            {
                                match.FirstPlayerSets++;
                                set.Player = match.SecondPlayer;
                                set.PlayerId = match.SecondPlayerId;
                                set.IsSetFinished = true;

                                if (match.SecondPlayerSets == 2)
                                {
                                    match.IsFinished = true;
                                    match.IsActive = false;

                                    //TODO Add Winner
                                }

                            }
                            else if (set.FirstPlayerGames == 6 && set.SecondPlayerGames == 6)
                            {
                                set.HasTieBreak = true;
                                set.TieBreak = new TieBreak()
                                {
                                    TieBreakPoints = new List<TieBreakPoint>()
                                        {
                                            new TieBreakPoint()
                                        }
                                };
                            }

                        }
                        else if (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints == 4)
                        {
                            point.FirstPlayerPoints = 3;
                            point.SecondPlayerPoints = 3;
                        }

                        game.Points.Add(point);
                    }
                }
                else
                {
                    if (IsFirstPlayer)
                    {
                        if ((tieBreakPoint.FirstPlayerPoint == 7 && tieBreakPoint.SecondPlayerpoint <= 5)
                            || ((tieBreakPoint.FirstPlayerPoint - 2 == tieBreakPoint.SecondPlayerpoint) && tieBreakPoint.SecondPlayerpoint > 5))
                        {
                            set.FirstPlayerGames++;
                            set.IsSetFinished = true;
                            set.Player = match.FirstPlayer;
                            set.PlayerId = match.FirstPlayerId;
                            match.Sets.Add(set);

                            if (match.Sets.Where(s => s.Player.Id == set.PlayerId).Count() == 2)
                            {
                                match.IsActive = false;
                                match.IsFinished = true;
                            }
                        }

                        tiebreak.TieBreakPoints.Add(tieBreakPoint);

                    }

                    if (IsSecondPlayer)
                    {
                        if ((tieBreakPoint.SecondPlayerpoint == 7 && tieBreakPoint.FirstPlayerPoint <= 5)
                            || ((tieBreakPoint.SecondPlayerpoint - 2 == tieBreakPoint.FirstPlayerPoint) && tieBreakPoint.FirstPlayerPoint > 5))
                        {
                            set.SecondPlayerGames++;
                            set.IsSetFinished = true;
                            set.Player = match.SecondPlayer;
                            set.PlayerId = match.SecondPlayerId;
                            match.Sets.Add(set);

                            if (match.Sets.Where(s => s.Player.Id == set.PlayerId).Count() == 2)
                            {
                                match.IsActive = false;
                                match.IsFinished = true;
                            }
                        }
                        tiebreak.TieBreakPoints.Add(tieBreakPoint);

                    }
                }
            }



            await this.context.SaveChangesAsync();

            var r = mapper.Map<UmpireResultViewModel>(match);

            return r;

        }

    }
}
