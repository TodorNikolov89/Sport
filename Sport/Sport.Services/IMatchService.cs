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

        Task<MatchScoreViewModel> Result(string buttonId, int matchId);

    }
}
