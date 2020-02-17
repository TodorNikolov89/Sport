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
    using ViewModels.Match;

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using System;

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


        //TODO mapper!!!

        /// <summary>
        /// This method returns all Tournaments from database
        /// </summary>
        /// <returns>Collection of AllTournamentsViewMOdel</returns>
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
                    IsStarted = t.IsStarted,
                    CreatorId = t.CreatorId,
                    Players = t.Players.Select(p => new UserViewModel
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
        }

        /// <summary>
        /// THis method returns Tournament by given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Tournament by given Id</returns>
        public TournamentFormModel ById(int id)
        {
            var tournament = this.context
            .Tournaments
            .Where(t => t.Id == id)
            .FirstOrDefault();

            var result = mapper.Map<TournamentFormModel>(tournament);

            return result;
        }

        /// <summary>
        /// This method creates tournament by given model and id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        public void Create(TournamentFormModel model, string userId)
        {
            var user = userManager
                .Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            if (user == null)
            {
                return;
            }

            var tournament = mapper.Map<Tournament>(model);
            tournament.Creator = user;
            tournament.CreatorId = user.Id;

            context.Tournaments.Add(tournament);
            context.SaveChanges();
        }

        /// <summary>
        /// This method deletes Tournaments by given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method edits  Tournament by given TournamentFormModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        public IEnumerable<MatchesViewModel> GetDrawPlayers(int id)
        {
            var matches = this.context
                  .Tournaments
                  .Where(t => t.Id == id)
                  .SelectMany(u => u.Matches)
                  .Include(u => u.FirstPlayer)
                  .Include(u => u.SecondPlayer)
                  .Include(u => u.Tournament)
                  .ToList();

            var result = mapper.Map<IEnumerable<MatchesViewModel>>(matches);

            return result;
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

        public void Signin(int id, User user)
        {
            var tournament = this.context
                .Tournaments
                .Include(u => u.Players)
                .ThenInclude(x => x.User)
                .FirstOrDefault(t => t.Id == id);


            if (tournament == null)
            {
                return; // TODO Return message 
            }

            if (tournament.Players.Any(u => u.UserId == user.Id))
            {
                return; // TODO Return message that user is already signed in
            }

            if (tournament.Players.Count() < tournament.NumberOfPlayers)
            {
                tournament.Players.Add(new UserTournament
                {
                    User = user,
                    UserId = user.Id,

                    Tournament = tournament,
                    TournamentId = tournament.Id
                });
            }
            else
            {
                return; //TODO return message "List of players is full"
            }

            this.context.SaveChanges();

        }

        public async Task Signout(int id, string userId)
        {
            var tournament = this.context
                .Tournaments
                .Where(t => t.Id == id)
                .Include(u => u.Players)
                .FirstOrDefault();

            UserTournament ut = tournament.Players
                .FirstOrDefault(x => x.UserId == userId);

            tournament.Players.Remove(ut);
            await this.context.SaveChangesAsync();
        }

        public void Start(int id)
        {
            var tournament = this.context
                 .Tournaments
                 .Include(u => u.Players)
                 .ThenInclude(x => x.User)
                 .FirstOrDefault(t => t.Id == id);

            var players = tournament
                .Players
                .Select(p => p.User)
                .ToList();

            //TODO Add some sorting to PLAYERS

            if (!tournament.Matches.Any())            {

                for (int i = 0; i < players.Count - 1; i += 2)
                {
                    tournament.Matches.Add(new Match
                    {
                        FirstPlayer = players[i],
                        FirstPlayerId = players[i].Id,

                        SecondPlayer = players[i + 1],
                        SecondPlayerId = players[i + 1].Id,

                        Tournament = tournament,
                        TournamentId = tournament.Id,


                        Sets = new List<Set>()
                            {
                                new Set()
                                {
                                   Games = new List<Game>()
                                        {
                                            new Game()
                                            {
                                                Points = new List<Point>()
                                                {

                                                }
                                            }
                                        }
                                }
                            }


                    });
                }

                tournament.IsStarted = true;
                this.context.SaveChanges();
            }

        }
    }
}
