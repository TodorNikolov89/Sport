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


        private static Task AddTournaments(FakeSportDbContext db)
        {
            return db.Add(
                new Tournament()
                {
                    Id = 1
                }, new Tournament()
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
