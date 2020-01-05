namespace Sport.Services
{
    using Domain;
    using ViewModels.Match;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMatchService
    {
        MatchScoreViewModel GetMatch(int id);

        Task<IEnumerable<Match>> GetAllActive();
        MatchScoreViewModel Result(string buttonId, int matchId, string firstPlayerPoints, int firstPlayerGames, int firstPlayerSets, int firstPlayerTieBreakPoints, string secondPlayerPoints, int secondPlayerGames, int secondPlayerSets, int secondPlayerTieBreakPoints);
        MatchScoreViewModel Result(MatchScoreViewModel model);
    }
}
