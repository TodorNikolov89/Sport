namespace Sport.Services.Implementation
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain;
    using ViewModels.Tournament;
    using ViewModels.User;
    using ViewModels.Player;

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

            var result = this.context
                .Tournaments
                .Select(t => new AllTournamentsViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    NumberOfPlayers = t.NumberOfPlayers,
                    AmmountOfMoney = t.AmmountOfMoney,
                    Place = t.Place,
                    Players = t.Users.Select(p => new UserViewModel
                    {
                        Id = p.User.Id,
                        DateOfBirth = p.User.DateOfBirth,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Email = p.User.Email

                    }).ToList()
                })
                .ToList();

            return result;
            //var allTournaments = this.context.Tournaments.ToList();

            //var result = mapper.Map<List<AllTournamentsViewModel>>(allTournaments);

            //return result;
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

        public IEnumerable<PlayerViewModel> GetTournamentPlayers(int id)
        {
            var users = this.context
                .Users
                .Where(t => t.Tournaments.Any(a => a.TournamentId == id))
                .ToList();

            var result = mapper.Map<IEnumerable<PlayerViewModel>>(users);

            return result;
        }

        public async Task Signin(int id, User user)
        {
            var tournament = this.context
                .Tournaments
                .Where(t => t.Id == id)
                .Include(u => u.Users)
                .FirstOrDefault();


            if (tournament == null)
            {
                return; // TODO Return message 
            }

            if (tournament.Users.Any(u => u.UserId == user.Id))
            {
                return; // TODO Return message that user is already signed in
            }

            if (tournament.Users.Count() < tournament.NumberOfPlayers)
            {
                tournament.Users.Add(new UserTournament
                {
                    User = user,
                    UserId = user.Id,

                    Tournament = tournament,
                    TournamentId = tournament.Id
                });
            }
            else
            {
                return;//TODO return message "List of players is full"
            }


            await this.context.SaveChangesAsync();
        }

        public async Task Signout(int id, string userId)
        {
            var tournament = this.context
                .Tournaments
                .Where(t => t.Id == id)
                .Include(u => u.Users)
                .FirstOrDefault();

            UserTournament ut = tournament.Users
                .FirstOrDefault(x => x.UserId == userId);

            tournament.Users.Remove(ut);
            await this.context.SaveChangesAsync();
        }
    }
}
