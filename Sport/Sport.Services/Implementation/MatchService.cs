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
        private static readonly string[] pointsArr = new[] { "0", "15", "30", "40", "Ad" };
        private static int playerToUpdatePoints = 0;
        private static int anotherPlayerPoints = 0;


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
                .Include(m => m.MatchResult)
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
                .Include(m => m.MatchResult)
                .FirstOrDefault(m => m.Id == id);

            var match = mapper.Map<MatchScoreViewModel>(dbMatch);

            return match;
        }


        public async Task<MatchScoreViewModel> Result(string buttonId, int matchId)
        {
            Set set = null;
            Game game = null;

            var match = await this.context
                .Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Umpire)
                .Include(m => m.MatchResult)
                .ThenInclude(mr => mr.Sets)
                .FirstOrDefaultAsync(m => m.Id == matchId);


            set = match.MatchResult.Sets.LastOrDefault();

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
                            }
                        }
                    }
                };

                match.MatchResult.Sets.Add(set);
            }

            var point = set.Games.LastOrDefault().LastPoint;

            var lastGame = set.LastGame;

            if (lastGame.IsGameFinished)
            {
                game = new Game();
            }


            if (buttonId.Equals("firstButtonId"))
            {
                game.FirsPlayerPoints++;
            }

            if (buttonId.Equals("secondButtonId"))
            {
                game.SecondPlayerPoints++;
            }

            if (!match.IsFinished)
            {
                if (((game.FirsPlayerPoints - 2 >= game.SecondPlayerPoints) && game.SecondPlayerPoints >= 3)
                || (game.FirsPlayerPoints == 4 && game.SecondPlayerPoints <= 2))
                {
                    lastGame.Points.Add(point);
                    lastGame.Player = match.FirstPlayer;
                    lastGame.PlayerId = match.FirstPlayerId;
                    lastGame.IsGameFinished = true;



                    GetCountersZero();

                    if ((playerToUpdateGames >= 6 && anotherPlayerGames <= 4)
                        || playerToUpdateGames == 7 && anotherPlayerGames == 5)
                    {
                        playerToUpdateSets++;

                        if (playerToUpdateSets == 2)
                        {
                            match.IsFinished = true;
                            match.IsActive = false;
                        }

                    }
                    else if (playerToUpdateGames == 6 && anotherPlayerGames == 6)
                    {
                        match.MatchResult.IsTieBreak = true;

                        //match.MatchResult.SecondPlayerPoints = 0;
                        //match.MatchResult.FirstPlayerPoints = 0;

                        //match.MatchResult.FirstPlayerGames = playerToUpdateGames;
                        //match.MatchResult.SecondPlayerGames = anotherPlayerGames;

                    }

                }
                else if (playerToUpdatePoints == 4 && anotherPlayerPoints == 4)
                {
                    playerToUpdatePoints = 3;
                    anotherPlayerPoints = 3;
                }




                if (buttonId.Equals("firstButtonId"))
                {
                    if (match.MatchResult.IsTieBreak)
                    {
                        match.MatchResult.FirstPlayerTieBreakPoints = playerToUpdatePoints;
                    }
                    else
                    {
                        //match.MatchResult.FirstPlayerPoints = playerToUpdateCounter;
                        //match.MatchResult.FirstPlayerGames = playerToUpdateGames;
                        //match.MatchResult.SecondPlayerPoints = anotherPlayerCounter;
                    }

                    //match.MatchResult.FirstPlayerSets = playerToUpdateSets;

                }
                else if (buttonId.Equals("secondButtonId"))
                {

                    if (match.MatchResult.IsTieBreak)
                    {
                        match.MatchResult.SecondPlayerTieBreakPoints = playerToUpdatePoints;
                    }
                    else
                    {
                        //match.MatchResult.SecondPlayerPoints = playerToUpdateCounter;
                        //match.MatchResult.SecondPlayerGames = playerToUpdateGames;
                        //match.MatchResult.FirstPlayerPoints = anotherPlayerCounter;
                    }

                    //match.MatchResult.SecondPlayerSets = playerToUpdateSets;

                }
            }


            await this.context.SaveChangesAsync();

            var result = mapper.Map<MatchScoreViewModel>(match);
            return result;
        }

        private static void GetCountersZero()
        {
            playerToUpdatePoints = 0;
            anotherPlayerPoints = 0;
        }

    }
}
