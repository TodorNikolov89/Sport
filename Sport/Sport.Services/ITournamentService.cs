using Sport.Domain;
using Sport.ViewModels.Player;
using Sport.ViewModels.Tournament;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sport.Services
{
    public interface ITournamentService
    {
        IEnumerable<AllTournamentsViewModel> All();

        void Create(TournamentFormModel model, string id);

        Task Edit(TournamentFormModel model);

        TournamentFormModel ById(int id);

        Task Delete(int id);

        Task Signin(int id, User user);

        Task Signout(int id, string userId);

        IEnumerable<PlayerViewModel> GetTournamentPlayers(int id);

        IEnumerable<PlayerViewModel> GetDrawPlayers(int id);
    }
}
