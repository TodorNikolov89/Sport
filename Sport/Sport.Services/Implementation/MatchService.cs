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
        private static readonly string firstPlayerPoint = "firstPlayerPoint";
        private static readonly string secondPlayerPoint = "secondPlayerPoint";
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

        public async Task<MatchScoreViewModel> Result(
            string buttonId,
            int matchId,
            string firstPlayerPoints,
            int firstPlayerGames,
            int firstPlayerSets,
            int firstPlayerTieBreakPoints,
            string secondPlayerPoints,
            int secondPlayerGames,
            int secondPlayerSets,
            int secondPlayerTieBreakPoints)
        {

            var match = await context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);

            //TODO 1 = 15, 2 = 30, 3 = 40, 4 = Ad, 5

            if (buttonId.Equals(firstPlayerPoint))
            {
                firstButtonClickCounter++;

                if (firstButtonClickCounter > 3 && secondButtonClickCounter <= 2)
                {
                    SetBothPlayersPointsToZero();

                    match.FirstPlayerGames++;
                    match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
                }
                else if (firstButtonClickCounter == 5 && secondButtonClickCounter == 3)
                {
                    SetBothPlayersPointsToZero();

                    match.FirstPlayerGames++;

                    UpdateResult(match);
                }
                else if (firstButtonClickCounter == 4 && secondButtonClickCounter == 4)
                {
                    firstButtonClickCounter = 3;
                    secondButtonClickCounter = 3;

                    UpdateResult(match);
                }
                else if (firstButtonClickCounter == 4 && secondButtonClickCounter == 3)
                {
                    match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
                }
                else if (firstButtonClickCounter == 3 && secondButtonClickCounter == 3)
                {
                    match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
                }
                else
                {
                    match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
                }
            }

            if (buttonId.Equals(secondPlayerPoint))
            {
                secondButtonClickCounter++;

                if (secondButtonClickCounter > 3 && firstButtonClickCounter <= 2)
                {
                    SetBothPlayersPointsToZero();

                    match.SecondPlayerGames++;
                    match.SecondPlayerPoints = pointsArr[secondButtonClickCounter];
                }
                else if (secondButtonClickCounter == 5 && firstButtonClickCounter == 3)
                {
                    SetBothPlayersPointsToZero();

                    match.SecondPlayerGames++;

                    UpdateResult(match);
                }
                else if (secondButtonClickCounter == 4 && firstButtonClickCounter == 4)
                {
                    firstButtonClickCounter = 3;
                    secondButtonClickCounter = 3;

                    UpdateResult(match);
                }
                else if (secondButtonClickCounter == 4 && firstButtonClickCounter == 3)
                {
                    match.SecondPlayerPoints = pointsArr[secondButtonClickCounter];
                }
                else if (secondButtonClickCounter == 3 && firstButtonClickCounter == 3)
                {
                    match.SecondPlayerPoints = pointsArr[secondButtonClickCounter];
                }
                else
                {
                    match.SecondPlayerPoints = pointsArr[secondButtonClickCounter];
                }
            }



            await this.context.SaveChangesAsync();

            var result = mapper.Map<MatchScoreViewModel>(match);
            return result;
        }

        private static void UpdateResult(Match match)
        {
            match.FirstPlayerPoints = pointsArr[firstButtonClickCounter];
            match.SecondPlayerPoints = pointsArr[secondButtonClickCounter];
        }

        private static void SetBothPlayersPointsToZero()
        {
            firstButtonClickCounter = 0;
            secondButtonClickCounter = 0;
        }
    }
}
