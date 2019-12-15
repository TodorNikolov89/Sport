using Sport.ViewModels.Tournament;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sport.Services
{
    public interface ITournamentService
    {
        IEnumerable<AllTournamentsViewModel> All();

        void Create(TournamentFormModel model);

        Task Edit(TournamentFormModel model);

        TournamentFormModel ById(int id);

        Task Delete(int id);

        Task Signin(int id);

    }
}
