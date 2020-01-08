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
    using System;

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


        public async Task<MatchScoreViewModel> Result(string buttonId, int matchId)
        {
            Set set = null;
            Game game = null;
            Point point = null;
            var lastMatchId = matchId;

            bool IsFirstPlayer = buttonId.Equals("firstButtonId");
            bool IsSecondPlayer = buttonId.Equals("secondButtonId");

            var match = await this.context
              .Matches
              .Include(m => m.FirstPlayer)
              .Include(m => m.SecondPlayer)
              .Include(m => m.Umpire)
              .Include(m => m.Sets)
              .ThenInclude(a=>a.Games)
              .ThenInclude(p=>p.Points)
              .FirstOrDefaultAsync(m => m.Id == matchId);
            

            set = match.Sets.ToList().LastOrDefault();
            

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
                                    FirsPlayerPoints=0,
                                    SecondPlayerPoints=0
                                }
                            }
                        }
                    }
                };

                match.Sets.Add(set);
            }

            game = set.Games.LastOrDefault();

            if (game == null || game.IsGameFinished)
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
            }

            var lastPoint = game.Points.ToList().LastOrDefault();

            
            point = new Point()
            {
                FirsPlayerPoints = lastPoint.FirsPlayerPoints,
                SecondPlayerPoints = lastPoint.SecondPlayerPoints,
                GameId = lastPoint.GameId,
                Game = lastPoint.Game
            };

            if (IsFirstPlayer)
            {
                if (!set.IsTieBreak)
                {
                    point.FirsPlayerPoints++;
                }
                else
                {
                    set.FirsPlayerTieBreakPoints++;
                }

            }

            if (IsSecondPlayer)
            {
                if (!set.IsTieBreak)
                {
                    point.SecondPlayerPoints++;
                }
                else
                {
                    set.SecondPlayerTieBreakPoints++;
                }
            }

            if (!match.IsFinished)
            {
                if (IsFirstPlayer)
                {
                    if (((point.FirsPlayerPoints - 2 >= point.SecondPlayerPoints) && point.SecondPlayerPoints >= 3)
                || (point.FirsPlayerPoints == 4 && point.SecondPlayerPoints <= 2))
                    {
                        game.Points.Add(point);
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
                            set.IsTieBreak = true;
                        }

                    }
                    else if (game.FirsPlayerPoints == 4 && game.SecondPlayerPoints == 4)
                    {
                        game.FirsPlayerPoints = 3;
                        game.SecondPlayerPoints = 3;
                    }

                    game.Points.Add(point);
                }



                if (IsSecondPlayer)
                {
                    if (((point.SecondPlayerPoints - 2 >= point.FirsPlayerPoints) && point.FirsPlayerPoints >= 3)
                || (point.SecondPlayerPoints == 4 && point.FirsPlayerPoints <= 2))
                    {
                        game.Points.Add(point);
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
                            set.IsTieBreak = true;
                        }

                    }
                    else if (game.FirsPlayerPoints == 4 && game.SecondPlayerPoints == 4)
                    {
                        game.FirsPlayerPoints = 3;
                        game.SecondPlayerPoints = 3;
                    }

                    game.Points.Add(point);
                }
            }


            await this.context.SaveChangesAsync();

            var result = mapper.Map<MatchScoreViewModel>(match);
            return result;

        }

    }
}
