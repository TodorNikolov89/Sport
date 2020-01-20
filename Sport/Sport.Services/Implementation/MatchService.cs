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
    using Microsoft.AspNetCore.Identity;
    using System;

    public class MatchService : IMatchService
    {
        private readonly SportDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public MatchService(SportDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Match>> GetAll()
        {
            var matches = await this.context
                .Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .ToListAsync();

            return matches;
        }

        public LiveResultViewModel GetMatch(int id)
        {
            var dbMatch = this.context
                .Matches
                .Include(m => m.Tournament)
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Umpire)
                .Include(m => m.Sets)
                .ThenInclude(a => a.Games)
                .ThenInclude(p => p.Points)
                .FirstOrDefault(m => m.Id == id);

            var match = mapper.Map<LiveResultViewModel>(dbMatch);

            return match;
        }

        public async Task<LiveResultViewModel> AddFirstPlayerPoint(int matchId)
        {
            Set set = null;
            Game game = null;
            Point point = null;
            TieBreakPoint tieBreakPoint = null;
            TieBreak tieBreak = null;

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
            game = set.Games.ToList().LastOrDefault();
            point = game.Points.ToList().LastOrDefault();


            if (!match.IsFinished)
            {
                if (!set.HasTieBreak)
                {
                    point.FirstPlayerPoints++;

                    if (((point.FirstPlayerPoints - 2 >= point.SecondPlayerPoints) && point.SecondPlayerPoints >= 3)
                        || (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints <= 2))
                    {
                        point.FirstPlayerPoints--;
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
                                match.FirstPlayer.Win++;
                                match.SecondPlayer.Loses++;
                                //TODO Add Winner
                            }
                            else
                            {
                                CreateGameAndSet(match);

                            }

                        }
                        else if (set.FirstPlayerGames == 6 && set.SecondPlayerGames == 6)
                        {
                            set.HasTieBreak = true;

                            set.TieBreak = new TieBreak
                            {
                                Set = set,
                                SetId = set.Id,
                                TieBreakPoints = new List<TieBreakPoint>()
                                    {
                                        new TieBreakPoint()
                                    }
                            };

                            PointsToZero(point);
                        }
                        else
                        {
                            AddNewGame(set);
                        }

                    }
                    else if (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints == 4)
                    {
                        point.FirstPlayerPoints = 3;
                        point.SecondPlayerPoints = 3;

                        game.Points.Add(point);
                    }
                }
                else
                {
                    tieBreak = await this.context
                          .TieBreaks
                          .Include(t => t.TieBreakPoints)
                          .FirstOrDefaultAsync(t => t.SetId == set.Id);

                    tieBreakPoint = tieBreak.TieBreakPoints.ToList().LastOrDefault(p => p.TieBreakId == tieBreak.Id);

                    tieBreakPoint.FirstPlayerPoint++;

                    if ((tieBreakPoint.FirstPlayerPoint == 7 && tieBreakPoint.SecondPlayerPoint <= 5)
                            || ((tieBreakPoint.FirstPlayerPoint - 2 == tieBreakPoint.SecondPlayerPoint) && tieBreakPoint.SecondPlayerPoint > 5))
                    {
                        set.FirstPlayerGames++;
                        match.FirstPlayerSets++;
                        set.IsSetFinished = true;
                        set.Player = match.FirstPlayer;
                        set.PlayerId = match.FirstPlayerId;
                        match.Sets.Add(set);

                        if (match.Sets.Where(s => s.Player.Id == set.PlayerId).Count() == 2)
                        {
                            match.IsActive = false;
                            match.IsFinished = true;
                            match.SecondPlayer.Win++;
                            match.FirstPlayer.Loses++;
                        }
                        else
                        {
                            CreateGameAndSet(match);
                        }
                    }

                    TieBreakPoint newTieBreakPoint = tieBreakPoint;
                    set.TieBreak.TieBreakPoints.Add(newTieBreakPoint);
                }
            }

            await this.context.SaveChangesAsync();
            var r = mapper.Map<LiveResultViewModel>(match);

            return r;

        }

        public async Task<LiveResultViewModel> AddSecondPlayerPoint(int matchId)
        {
            Set set = null;
            Game game = null;
            Point point = null;
            TieBreakPoint tieBreakPoint = null;
            TieBreak tieBreak = null;

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
            game = set.Games.ToList().LastOrDefault();
            point = game.Points.ToList().LastOrDefault();


            if (!match.IsFinished)
            {
                if (!set.HasTieBreak)
                {
                    point.SecondPlayerPoints++;

                    if (((point.SecondPlayerPoints - 2 >= point.FirstPlayerPoints) && point.FirstPlayerPoints >= 3)
                        || (point.SecondPlayerPoints == 4 && point.FirstPlayerPoints <= 2))
                    {
                        point.SecondPlayerPoints--;
                        game.Points.Add(point);


                        game.Player = match.SecondPlayer;
                        game.PlayerId = match.SecondPlayerId;
                        game.IsGameFinished = true;

                        set.SecondPlayerGames++;
                        set.Games.Add(game);


                        if ((set.SecondPlayerGames >= 6 && set.FirstPlayerGames <= 4)
                            || set.SecondPlayerGames == 7 && set.FirstPlayerGames == 5)
                        {
                            match.SecondPlayerSets++;
                            set.Player = match.SecondPlayer;
                            set.PlayerId = match.SecondPlayerId;
                            set.IsSetFinished = true;

                            if (match.SecondPlayerSets == 2)
                            {
                                match.IsFinished = true;
                                match.IsActive = false;

                                //TODO Add Winner
                            }
                            else
                            {
                                CreateGameAndSet(match);

                            }

                        }
                        else if (set.SecondPlayerGames == 6 && set.FirstPlayerGames == 6)
                        {
                            set.HasTieBreak = true;

                            set.TieBreak = new TieBreak
                            {
                                Set = set,
                                SetId = set.Id,
                                TieBreakPoints = new List<TieBreakPoint>()
                                    {
                                        new TieBreakPoint()
                                    }
                            };

                            PointsToZero(point);
                        }
                        else
                        {
                            AddNewGame(set);
                        }

                    }
                    else if (point.SecondPlayerPoints == 4 && point.FirstPlayerPoints == 4)
                    {
                        point.FirstPlayerPoints = 3;
                        point.SecondPlayerPoints = 3;

                        game.Points.Add(point);
                    }
                }
                else
                {
                    tieBreak = await this.context
                           .TieBreaks
                           .Include(t => t.TieBreakPoints)
                           .FirstOrDefaultAsync(t => t.SetId == set.Id);

                    tieBreakPoint = tieBreak.TieBreakPoints.ToList().LastOrDefault(p => p.TieBreakId == tieBreak.Id);

                    tieBreakPoint.SecondPlayerPoint++;

                    if ((tieBreakPoint.SecondPlayerPoint == 7 && tieBreakPoint.FirstPlayerPoint <= 5)
                            || ((tieBreakPoint.SecondPlayerPoint - 2 == tieBreakPoint.FirstPlayerPoint) && tieBreakPoint.FirstPlayerPoint > 5))
                    {
                        set.SecondPlayerGames++;
                        match.SecondPlayerSets++;
                        set.IsSetFinished = true;
                        set.Player = match.SecondPlayer;
                        set.PlayerId = match.SecondPlayerId;
                        match.Sets.Add(set);

                        if (match.Sets.Where(s => s.Player.Id == set.PlayerId).Count() == 2)
                        {
                            match.IsActive = false;
                            match.IsFinished = true;
                        }
                        else
                        {
                            CreateGameAndSet(match);
                        }
                    }

                    TieBreakPoint newTieBreakPoint = tieBreakPoint;
                    set.TieBreak.TieBreakPoints.Add(newTieBreakPoint);
                }
            }

            await this.context.SaveChangesAsync();

            var r = mapper.Map<LiveResultViewModel>(match);

            return r;

        }

        private static void PointsToZero(Point point)
        {
            point.FirstPlayerPoints = 0;
            point.SecondPlayerPoints = 0;
        }

        private static void AddNewGame(Set set)
        {
            Game newG = new Game();
            Point newP = new Point();
            newG.Points.Add(newP);
            set.Games.Add(newG);
        }

        private static void CreateGameAndSet(Match match)
        {
            Point newPoint = new Point();
            Game newGame = new Game();
            Set newSet = new Set();

            newGame.Points.Add(newPoint);
            newSet.Games.Add(newGame);
            match.Sets.Add(newSet);
        }

        public void AddUmpire(int id, string userId)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id.Equals(userId));

            var match = this.context
                .Matches
                .FirstOrDefault(m => m.Id == id);

            if (match == null)
            {
                return;
            }

            match.Umpire = user;
            match.UmpireId = user.Id;

            this.context.SaveChanges();
        }


    }
}
