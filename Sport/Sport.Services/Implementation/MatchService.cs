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
        private static readonly string AddButtonIdFirstPlayer = "AddButtonIdFirstPlayer";
        private static readonly string AddButtonIdSecondPlayer = "AddButtonIdSecondPlayer";
        private static readonly string[] pointsArr = new[] { "0", "15", "30", "40", "Ad" };
        private static int firstButtonClickCounter = 0;
        private static int secondButtonClickCounter = 0;


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

        //public async Task<MatchScoreViewModel> Result(
        //    string buttonId,
        //    int matchId,
        //    string firstPlayerPoints,
        //    int firstPlayerGames,
        //    int firstPlayerSets,
        //    int firstPlayerTieBreakPoints,
        //    string secondPlayerPoints,
        //    int secondPlayerGames,
        //    int secondPlayerSets,
        //    int secondPlayerTieBreakPoints)
        //{

        //    var match = await context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);

        //    //TODO 1 = 15, 2 = 30, 3 = 40, 4 = Ad, 5

        //    if (buttonId.Equals(AddButtonIdFirstPlayer))
        //    {
        //        firstButtonClickCounter++;

        //        if (firstButtonClickCounter > 3 && secondButtonClickCounter <= 2)
        //        {
        //            SetBothPlayersPointsToZero();

        //            match.FirstPlayerGames++;
        //            match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
        //        }
        //        else if (firstButtonClickCounter == 5 && secondButtonClickCounter == 3)
        //        {
        //            SetBothPlayersPointsToZero();

        //            match.FirstPlayerGames++;

        //            UpdateResult(match);
        //        }
        //        else if (firstButtonClickCounter == 4 && secondButtonClickCounter == 4)
        //        {
        //            firstButtonClickCounter = 3;
        //            secondButtonClickCounter = 3;

        //            UpdateResult(match);
        //        }
        //        else if (firstButtonClickCounter == 4 && secondButtonClickCounter == 3)
        //        {
        //            match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
        //        }
        //        else if (firstButtonClickCounter == 3 && secondButtonClickCounter == 3)
        //        {
        //            match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
        //        }
        //        else
        //        {
        //            match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
        //        }
        //    }



        //    await this.context.SaveChangesAsync();

        //    var result = mapper.Map<MatchScoreViewModel>(match);
        //    return result;
        //}


        public async Task<MatchScoreViewModel> Result(string buttonId, int matchId)
        {
            var match = await context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);

            string buttonName = string.Empty;
            string playerToUpdatePoints = string.Empty;
            int? playerToUpdateGames = 0;
            int? anotherPlayerGames = 0;
            int? playerToUpdateSets = 0;
            string anotherPlayerPoints = string.Empty;
            int playerToUpdateCounter = 0;
            int anotherPlayerCounter = 0;

            if (buttonId.Equals(AddButtonIdFirstPlayer) && match.IsTieBreak == false)
            {
                playerToUpdatePoints = match.FirstPlayerPoints;
                playerToUpdateGames = match.FirstPlayerGames;
                anotherPlayerGames = match.SecondPlayerGames;
                playerToUpdateSets = match.FirstPlayerSets;
                anotherPlayerPoints = match.SecondPlayerPoints;

                firstButtonClickCounter++;

                playerToUpdateCounter = firstButtonClickCounter;
                anotherPlayerCounter = secondButtonClickCounter;


            }
            else if (buttonId.Equals(AddButtonIdSecondPlayer) && match.IsTieBreak == false)
            {
                playerToUpdatePoints = match.SecondPlayerPoints;
                playerToUpdateGames = match.SecondPlayerGames;
                anotherPlayerGames = match.FirstPlayerGames;
                playerToUpdateSets = match.SecondPlayerSets;
                anotherPlayerPoints = match.FirstPlayerPoints;

                secondButtonClickCounter++;

                playerToUpdateCounter = secondButtonClickCounter;
                anotherPlayerCounter = firstButtonClickCounter;
            }

            //TODO 1 = 15, 2 = 30, 3 = 40, 4 = Ad, 5           

            if (!match.IsTieBreak)
            {
                if (playerToUpdateCounter > 3 && anotherPlayerCounter <= 2)
                {
                    SetBothPlayersPointsToZero();

                    // match.FirstPlayerGames++;
                    playerToUpdateGames++;

                    //////////////////
                    if (playerToUpdateGames == 6 && anotherPlayerGames < 5)
                    {
                        playerToUpdateSets++;
                        playerToUpdateGames = 0;

                        if (playerToUpdateSets == 2)
                        {
                            //TODO logic in view
                            match.IsFinished = true;
                        }
                    }
                    else if (playerToUpdateGames > 6 && anotherPlayerGames == 5)
                    {
                        playerToUpdateSets++;
                        playerToUpdateGames = 0;

                        if (playerToUpdateSets == 2)
                        {
                            match.IsFinished = true;
                            SetBothPlayersPointsToZero();
                        }
                    }
                    else if (playerToUpdateGames == 6 && anotherPlayerGames == 6)
                    {
                        match.IsTieBreak = true;
                    }
                    //////////////////
                    ///
                    playerToUpdatePoints = pointsArr[firstButtonClickCounter];
                    anotherPlayerPoints = pointsArr[secondButtonClickCounter];
                }
                else if (playerToUpdateCounter == 5 && anotherPlayerCounter == 3)
                {
                    SetBothPlayersPointsToZero();

                    playerToUpdateGames++;

                    //////////////////
                    if (playerToUpdateGames == 6 && anotherPlayerGames < 5)
                    {
                        playerToUpdateSets++;
                        playerToUpdateGames = 0;

                        if (playerToUpdateSets == 2)
                        {
                            //TODO logic in view
                            match.IsFinished = true;
                        }
                    }
                    else if (playerToUpdateGames > 6 && anotherPlayerGames == 5)
                    {
                        playerToUpdateSets++;
                        playerToUpdateGames = 0;

                        if (playerToUpdateSets == 2)
                        {
                            match.IsFinished = true;
                        }
                    }
                    else if (playerToUpdateGames == 6 && anotherPlayerGames == 6)
                    {
                        match.IsTieBreak = true;
                    }
                    //////////////////
                    playerToUpdatePoints = pointsArr[firstButtonClickCounter];
                    anotherPlayerPoints = pointsArr[secondButtonClickCounter];
                }
                else if (playerToUpdateCounter == 4 && anotherPlayerCounter == 4)
                {
                    firstButtonClickCounter = 3;
                    secondButtonClickCounter = 3;

                    playerToUpdateCounter = 3;
                    anotherPlayerCounter = 3;

                    playerToUpdatePoints = pointsArr[playerToUpdateCounter];
                    anotherPlayerPoints = pointsArr[anotherPlayerCounter];
                }
                else if (playerToUpdateCounter == 4 && anotherPlayerCounter == 3)
                {
                    playerToUpdatePoints = pointsArr[playerToUpdateCounter];
                }
                else if (playerToUpdateCounter == 3 && anotherPlayerCounter == 3)
                {
                    playerToUpdatePoints = pointsArr[playerToUpdateCounter];
                }
                else
                {
                    playerToUpdatePoints = pointsArr[playerToUpdateCounter];
                }
            }

            if (buttonId.Equals(AddButtonIdFirstPlayer))
            {
                match.FirstPlayerPoints = playerToUpdatePoints;
                match.FirstPlayerGames = playerToUpdateGames;
                match.FirstPlayerSets = playerToUpdateSets;
                match.SecondPlayerPoints = anotherPlayerPoints;
            }
            else if (buttonId.Equals(AddButtonIdSecondPlayer))
            {
                match.SecondPlayerPoints = playerToUpdatePoints;
                match.SecondPlayerGames = playerToUpdateGames;
                match.SecondPlayerSets = playerToUpdateSets;
                match.FirstPlayerPoints = anotherPlayerPoints;
            }

            await this.context.SaveChangesAsync();

            var result = mapper.Map<MatchScoreViewModel>(match);
            return result;
        }

        private static void SetBothPlayersPointsToZero()
        {
            firstButtonClickCounter = 0;
            secondButtonClickCounter = 0;
        }

    }
}
