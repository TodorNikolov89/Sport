namespace Sport.Services
{
    using Domain;
    using ViewModels.Player;
    using ViewModels.Tournament;
    using ViewModels.Match;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITournamentService
    {
        IEnumerable<AllTournamentsViewModel> All();

        Task Create(TournamentFormModel model, string id);

        Task Edit(TournamentFormModel model);

        TournamentFormModel ById(int id);

        Task Delete(int id);

        void Signin(int id, User user);

        Task Signout(int id, string userId);

        IEnumerable<PlayerViewModel> GetTournamentPlayers(int id);

        IEnumerable<MatchesViewModel> GetDrawPlayers(int id);

        void Start(int id);
    }
}
