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
        private static int playerToUpdateCounter = 0;
        private static int anotherPlayerCounter = 0;


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
            var match = await context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);

            int playerToUpdateGames = 0;
            int anotherPlayerGames = 0;
            int playerToUpdateSets = 0;
            int anotherPlayerSets = 0;


            if (buttonId.Equals("firstButtonId"))
            {
                if (!match.IsTieBreak)
                {
                    playerToUpdateCounter = Array.IndexOf(pointsArr, match.FirstPlayerPoints);
                    anotherPlayerCounter = Array.IndexOf(pointsArr, match.SecondPlayerPoints);
                }
                else
                {
                    playerToUpdateCounter = match.FirstPlayerTieBreakPoints;
                    anotherPlayerCounter = match.SecondPlayerTieBreakPoints;
                }

                playerToUpdateGames = match.FirstPlayerGames;
                anotherPlayerGames = match.SecondPlayerGames;

                playerToUpdateSets = match.FirstPlayerSets;
                anotherPlayerSets = match.SecondPlayerSets;

                playerToUpdateCounter++;
            }

            if (buttonId.Equals("secondButtonId"))
            {
                if (!match.IsTieBreak)
                {
                    playerToUpdateCounter = Array.IndexOf(pointsArr, match.SecondPlayerPoints);
                    anotherPlayerCounter = Array.IndexOf(pointsArr, match.FirstPlayerPoints);
                }
                else
                {
                    playerToUpdateCounter = match.SecondPlayerTieBreakPoints;
                    anotherPlayerCounter = match.FirstPlayerTieBreakPoints;
                }


                playerToUpdateGames = match.SecondPlayerGames;
                anotherPlayerGames = match.FirstPlayerGames;

                playerToUpdateSets = match.SecondPlayerSets;
                anotherPlayerSets = match.FirstPlayerSets;

                playerToUpdateCounter++;
            }



            if (!match.IsFinished)
            {
                if (!match.IsTieBreak)
                {
                    if (((playerToUpdateCounter - 2 >= anotherPlayerCounter) && anotherPlayerCounter >= 3)
                    || (playerToUpdateCounter == 4 && anotherPlayerCounter <= 2))
                    {
                        playerToUpdateGames++;

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
                            match.IsTieBreak = true;

                            match.SecondPlayerPoints = pointsArr[0];
                            match.FirstPlayerPoints = pointsArr[0];

                            match.FirstPlayerGames = playerToUpdateGames;
                            match.SecondPlayerGames = anotherPlayerGames;

                        }
                        //TODO Check sets
                    }
                    else if (playerToUpdateCounter == 4 && anotherPlayerCounter == 4)
                    {
                        playerToUpdateCounter = 3;
                        anotherPlayerCounter = 3;
                    }
                }
                else if ((playerToUpdateCounter >= 7 && anotherPlayerCounter <= 5)
                        || (playerToUpdateCounter - 2 == anotherPlayerCounter && anotherPlayerCounter > 5))
                {

                    playerToUpdateSets++;

                    if (playerToUpdateSets == 2)
                    {
                        match.IsFinished = true;
                        match.IsActive = false;
                    }

                    playerToUpdateCounter = 0;
                    anotherPlayerCounter = 0;

                    playerToUpdateGames = 0;
                    anotherPlayerGames = 0;

                    match.FirstPlayerGames = playerToUpdateGames;
                    match.SecondPlayerGames = anotherPlayerGames;

                    match.FirstPlayerTieBreakPoints = playerToUpdateGames;
                    match.SecondPlayerTieBreakPoints = anotherPlayerGames;

                    match.IsTieBreak = false;
                }


                if (buttonId.Equals("firstButtonId"))
                {
                    if (match.IsTieBreak)
                    {
                        match.FirstPlayerTieBreakPoints = playerToUpdateCounter;
                    }
                    else
                    {
                        match.FirstPlayerPoints = pointsArr[playerToUpdateCounter];
                        match.FirstPlayerGames = playerToUpdateGames;
                        match.SecondPlayerPoints = pointsArr[anotherPlayerCounter];
                    }

                    match.FirstPlayerSets = playerToUpdateSets;

                }
                else if (buttonId.Equals("secondButtonId"))
                {

                    if (match.IsTieBreak)
                    {
                        match.SecondPlayerTieBreakPoints = playerToUpdateCounter;
                    }
                    else
                    {
                        match.SecondPlayerPoints = pointsArr[playerToUpdateCounter];
                        match.SecondPlayerGames = playerToUpdateGames;
                        match.FirstPlayerPoints = pointsArr[anotherPlayerCounter];
                    }

                    match.SecondPlayerSets = playerToUpdateSets;

                }
            }


            await this.context.SaveChangesAsync();

            var result = mapper.Map<MatchScoreViewModel>(match);
            return result;
        }

        private static void GetCountersZero()
        {
            playerToUpdateCounter = 0;
            anotherPlayerCounter = 0;
        }

    }
}
