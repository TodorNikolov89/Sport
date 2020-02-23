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

        public MatchService(SportDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// This method returns all matches
        /// </summary>
        /// <returns>
        /// Collection of type AllMatchesViewModel
        /// </returns>
        public async Task<IEnumerable<AllMatchesViewModel>> GetAll()
        {
            var matches = await this.context
                .Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .ToListAsync();

            var result = mapper.Map<IEnumerable<AllMatchesViewModel>>(matches);


            return result;
        }

        /// <summary>
        /// This method returns match by the given id
        /// </summary>
        /// <param name="id">Id of the match</param>
        /// <returns>Returns match by the given id</returns>
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

        /// <summary>
        /// Add point to the first player of match by the given Id
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns> Task<LiveREsultViewModel> </returns>
        public async Task<LiveResultViewModel> AddFirstPlayerPoint(int matchId)
        {
            Set set = null;
            Game game = null;
            Point point = new Point();
            TieBreakPoint tieBreakPoint = null;
            TieBreak tieBreak = null;

            Match match = await GetCurrentMatch(matchId);

            set = match.Sets.ToList().LastOrDefault();
            game = set.Games.ToList().LastOrDefault();
            var lastPoint = game.Points.ToList().LastOrDefault();

            PointValue(game, point, lastPoint);

            if (!match.IsFinished)
            {
                if (!set.HasTieBreak)
                {
                    point.FirstPlayerPoints++;

                    if (((point.FirstPlayerPoints - 2 >= point.SecondPlayerPoints) && point.SecondPlayerPoints >= 3)
                        || (point.FirstPlayerPoints == 4 && point.SecondPlayerPoints <= 2))
                    {
                        point.FirstPlayerPoints--;
                        //game.Points.Add(point);

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
                                PointsToZero(point);
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
                    else
                    {
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

                    AddTieBreakPoint(tieBreakPoint, tieBreak);
                }
            }

            await this.context.SaveChangesAsync();
            var r = mapper.Map<LiveResultViewModel>(match);

            return r;

        }

        /// <summary>
        /// Add point to the second player of match by the given Id
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns> Task<LiveREsultViewModel> </returns>
        public async Task<LiveResultViewModel> AddSecondPlayerPoint(int matchId)
        {
            Set set = null;
            Game game = null;
            Point point = new Point();
            TieBreakPoint tieBreakPoint = null;
            TieBreak tieBreak = null;

            Match match = await GetCurrentMatch(matchId);

            set = match.Sets.ToList().LastOrDefault();
            game = set.Games.ToList().LastOrDefault();
            var lastPoint = game.Points.ToList().LastOrDefault();

            //Refactor this in both methods
            PointValue(game, point, lastPoint);

            if (!match.IsFinished)
            {
                if (!set.HasTieBreak)
                {
                    point.SecondPlayerPoints++;

                    if (((point.SecondPlayerPoints - 2 >= point.FirstPlayerPoints) && point.FirstPlayerPoints >= 3)
                        || (point.SecondPlayerPoints == 4 && point.FirstPlayerPoints <= 2))
                    {
                        point.SecondPlayerPoints--;
                        //game.Points.Add(point);


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
                    else
                    {
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

                    AddTieBreakPoint(tieBreakPoint, tieBreak);
                }
            }

            await this.context.SaveChangesAsync();

            var r = mapper.Map<LiveResultViewModel>(match);

            return r;

        }

        private static void AddTieBreakPoint(TieBreakPoint tieBreakPoint, TieBreak tieBreak)
        {
            TieBreakPoint newTieBreakPoint = new TieBreakPoint()
            {
                FirstPlayerPoint = tieBreakPoint.FirstPlayerPoint,
                SecondPlayerPoint = tieBreakPoint.SecondPlayerPoint,
                TieBreak = tieBreakPoint.TieBreak,
                TieBreakId = tieBreakPoint.TieBreakId
            };

            tieBreak.TieBreakPoints.Add(newTieBreakPoint);
        }

        /// <summary>
        /// This method gets user by the given userId and adds it as an umpiree to the given match by matchId
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="userId"></param>
        public void AddUmpire(int matchId, string userId)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id.Equals(userId));

            var match = this.context
                .Matches
                .FirstOrDefault(m => m.Id == matchId);

            if (match == null)
            {
                return;
            }

            match.Umpire = user;
            //match.UmpireId = user.Id;
            match.IsActive = true;

            this.context.SaveChanges();
        }

        /// <summary>
        /// This method returns collection of all matches that are not finished
        /// </summary>
        /// <returns>Collection of all the matches that are not finished</returns>
        public async Task<IEnumerable<LiveMatchesViewModel>> GetLiveMatches()
        {
            var allLiveMatches = await this.context
                .Matches
                .Where(m => m.IsActive == true)
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Sets)
                .ThenInclude(a => a.Games)
                .ThenInclude(p => p.Points)
                .ToListAsync();

            var result = mapper.Map<List<LiveMatchesViewModel>>(allLiveMatches);

            return result;
        }

        /// <summary>
        /// This method returns collection of all matches that are finished
        /// </summary>
        /// <returns>Collection of all matches that are finished</returns>
        public async Task<IEnumerable<FinishedMatchesViewModel>> GetFinishedMatches()
        {
            var allFinishedMatches = await this.context
                .Matches
                .Where(m => m.IsFinished == true)
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Sets)
                .ThenInclude(a => a.Games)
                .ThenInclude(p => p.Points)
                .ToListAsync();

            var result = mapper.Map<List<FinishedMatchesViewModel>>(allFinishedMatches);

            return result;
        }


        public async Task<Match> GetCurentMatchDetais(int matchId)
        {
            var dbMatch = await this.context
               .Matches
               .Include(m => m.FirstPlayer)
               .Include(m => m.SecondPlayer)
               .Include(m => m.Umpire)
               .Include(m => m.Tournament)
               .Include(m => m.Sets)
               .ThenInclude(t => t.TieBreak)
               .ThenInclude(p => p.TieBreakPoints)
               .Include(s => s.Sets)
               .ThenInclude(g => g.Games)
               .ThenInclude(p => p.Points)
               .FirstOrDefaultAsync(m => m.Id == matchId);

            // var match = mapper.Map<FinishedMatchDetaisViewModel>(dbMatch);

            return dbMatch;
        }

        /// <summary>
        /// This method checks if last point is null and adds game and game id to the new point or new point gets last point values
        /// </summary>
        /// <param name="game"></param>
        /// <param name="point"></param>
        /// <param name="lastPoint"></param>
        private static void PointValue(Game game, Point point, Point lastPoint)
        {
            if (lastPoint == null || (lastPoint.FirstPlayerPoints == 0 && lastPoint.SecondPlayerPoints == 0))
            {
                point.Game = game;
                point.GameId = game.Id;
            }
            else
            {
                point.Game = lastPoint.Game;
                point.GameId = lastPoint.GameId;
                point.FirstPlayerPoints = lastPoint.FirstPlayerPoints;
                point.SecondPlayerPoints = lastPoint.SecondPlayerPoints;
            }
        }

        /// <summary>
        /// This method make firstPlayerPoints and secondPlayerPoints equals to zero from the given Point
        /// </summary>
        /// <param name="point"></param>
        private static void PointsToZero(Point point)
        {
            point.FirstPlayerPoints = 0;
            point.SecondPlayerPoints = 0;
        }

        /// <summary>
        /// This Method adds a new game in the given set
        /// </summary>
        /// <param name="set"></param>
        private static void AddNewGame(Set set)
        {
            Game newG = new Game();
           // Point newP = new Point();
            //newG.Points.Add(newP);
            set.Games.Add(newG);
        }

        /// <summary>
        /// This method creates new game and set. 
        /// </summary>
        /// <param name="match"></param>
        private static void CreateGameAndSet(Match match)
        {
            Point newPoint = new Point();
            Game newGame = new Game();
            Set newSet = new Set();

            newSet.Games.Add(newGame);
            match.Sets.Add(newSet);
        }

        private async Task<Match> GetCurrentMatch(int matchId)
        {
            return await this.context
            .Matches
            .Include(m => m.FirstPlayer)
            .Include(m => m.SecondPlayer)
            .Include(m => m.Umpire)
            .Include(m => m.Sets)
            .ThenInclude(a => a.Games)
            .ThenInclude(p => p.Points)
            .FirstOrDefaultAsync(m => m.Id == matchId);
        }




    }
}
