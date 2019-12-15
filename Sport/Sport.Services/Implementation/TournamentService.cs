namespace Sport.Services.Implementation
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Sport.Data;
    using Sport.Domain;
    using Sport.ViewModels.Tournament;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TournamentService : ITournamentService
    {
        private readonly IMapper mapper;
        private readonly SportDbContext context;
        private readonly UserManager<User> userManager;

        public TournamentService(IMapper mapper, SportDbContext context, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
        }

        public IEnumerable<AllTournamentsViewModel> All()
        {
            return this.context
                .Tournaments
                .Select(t => new AllTournamentsViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                });
        }

        public TournamentFormModel ById(int id)
        {
            var tournament = this.context
            .Tournaments
            .Where(t => t.Id == id)
            .FirstOrDefault();

            var result = mapper.Map<TournamentFormModel>(tournament);

            return result;
        }


        public void Create(TournamentFormModel model)
        {
            var tournament = mapper.Map<Tournament>(model);

            context.Tournaments.Add(tournament);
            context.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var tournament = await this.context.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return;
            }

            this.context.Tournaments.Remove(tournament);
            await this.context.SaveChangesAsync();
        }

        public async Task Edit(TournamentFormModel model)
        {
            var tournament = await this.context
                .Tournaments.FindAsync(model.Id);

            if (tournament == null)
            {
                return;
            }

            tournament = mapper.Map(model, tournament);
            this.context.SaveChanges();

        }

        public async Task Signin(int id)
        {
            return;
        }
    }
}
