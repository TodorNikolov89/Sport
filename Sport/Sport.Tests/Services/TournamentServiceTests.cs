namespace Sport.Tests.Services
{
    using Fakes;
    using Domain;
    using Profiles;
    using Sport.Services;
    using Sport.Services.Implementation;

    using System.Threading.Tasks;
    using AutoMapper;
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System;
    using Sport.ViewModels.Tournament;
    using System.Collections.Generic;

    public class TournamentServiceTests
    {

        [Fact]
        public async Task AllShouldReturnAllTournaments()
        {
            //Arrange
            const string databaseName = "TournamentAll";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var userManager = GetMockUserManager();

            ITournamentService service = new TournamentService(mapper, db.Data, userManager.Object);

            //Act
            var expectedTournamentsCount = 2;

            var actualTournamentsCount = service.All().ToList().Count();

            //Assert
            Assert.True(expectedTournamentsCount == actualTournamentsCount,
                $"Expected count ot tournaments should be {expectedTournamentsCount} not {actualTournamentsCount}!");
        }

        [Fact]
        public async Task ByIdShouldReturnTournamentByGivenId()
        {
            //Arrange
            const string databaseName = "TournamentById";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var userManager = GetMockUserManager();

            ITournamentService service = new TournamentService(mapper, db.Data, userManager.Object);

            //Act
            var expectedTournamentId = 1;
            var actualTournament = service.ById(expectedTournamentId);

            //Assert
            Assert.True(expectedTournamentId == actualTournament.Id);
        }


        [Fact]
        public async Task DeleteShouldRemoveTournamentById()
        {
            //Arrange
            const string databaseName = "TournamentDelete";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);
            //TODO userManager is not used

            var userManager = GetMockUserManager();

            ITournamentService service = new TournamentService(mapper, db.Data, userManager.Object);

            //Act
            var tournamentId = 1;
            await service.Delete(tournamentId);

            var tournament = GetTournament(db, tournamentId);

            //Assert
            Assert.True(tournament == null);
        }



        [Fact]
        public async Task EditShouldEditTournamentByModel()
        {
            //Arrange
            const string databaseName = "TournamentDelete";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);
            //TODO userManager is not used

            var userManager = GetMockUserManager();

            ITournamentService service = new TournamentService(mapper, db.Data, userManager.Object);

            //Act
            int tournamentId = 1;
            TournamentFormModel expectedTournament = new TournamentFormModel()
            {
                Id = tournamentId,
                Name = "SofiaOpen",
                Place = "Sofia"
            };

            await service.Edit(expectedTournament);

            var actualTournament = GetTournament(db, tournamentId);

            //Assert
            Assert.Equal(expectedTournament.Name, actualTournament.Name);
            Assert.Equal(expectedTournament.Place, actualTournament.Place);

        }

        [Fact]
        public async Task EditShouldReturnTournamentNotfound()
        {
            //Arrange
            const string databaseName = "TournamentDeleteNotfound";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);
            //TODO userManager is not used

            var userManager = GetMockUserManager();

            ITournamentService service = new TournamentService(mapper, db.Data, userManager.Object);

            //Act
            int tournamentId = -1;
            TournamentFormModel expectedTournament = new TournamentFormModel()
            {
                Id = tournamentId,
                Name = "SofiaOpen",
                Place = "Sofia"
            };

            await service.Edit(expectedTournament);

            var actualTournament = GetTournament(db, tournamentId);

            //Assert
            Assert.True(actualTournament == null);

        }

        [Fact]
        public async Task SigninShouldSignInCurrentUser()
        {
            //Arrange
            const string databaseName = "TournamentDeleteNotfound";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);
            //TODO userManager is not used

            var userManager = GetMockUserManager();

            ITournamentService service = new TournamentService(mapper, db.Data, userManager.Object);

        }


        private static Tournament GetTournament(FakeSportDbContext db, int tournamentId)
        {
            return db.Data.Tournaments.FirstOrDefault(t => t.Id == tournamentId);
        }

        private static Task AddTournaments(FakeSportDbContext db)
        {
            return db.Add(
                new Tournament()
                {
                    Id = 1,
                    Name = "SofiaOpen",
                    Place = "Sofia",                   
                },
                new Tournament()
                {
                    Id = 2
                });
        }

        private static Task AddUsers(FakeSportDbContext db)
        {
            return db.Add(
                new User()
                {
                    Id = "1"
                }, new User()
                {
                    Id = "2"
                });
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
