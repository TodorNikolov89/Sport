namespace Sport.Tests
{
    using Profiles;
    using Domain;
    using Sport.Services.Implementation;
    using Sport.Tests.Fakes;

    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;
    using Xunit;

    public class MatchServiceTests
    {
        [Fact]
        public async Task GetMatchShouldReturnTrueIfMatchExists()
        {
            //Arrange
            const string databaseName = "MatchGetMatch";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            await SeedData(db);

            var matchService = new MatchService(db.Data, mapper);

            //Act
            var expectedResultMatchWithId = 2;
            var actualResult = matchService.GetMatch(expectedResultMatchWithId);

            //Assert
            Assert.True(expectedResultMatchWithId == actualResult.Id, "The match does not exist!");
        }

        [Fact]
        public async Task GetMatchShouldReturnTrueIfMatchDoesNotExist()
        {
            //Arrange
            const string databaseName = "MatchGetMatchDoesNotExist";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            await SeedData(db);

            var matchService = new MatchService(db.Data, mapper);

            //Act
            var expectedResultMatchWithId = db.Data.Matches.Count() + 1;
            var actualResult = matchService.GetMatch(expectedResultMatchWithId);

            //Assert
            Assert.True(actualResult == null, "The match exists!");
        }

        [Fact]
        public async Task GetAllShouldReturnCollectionWithCountOf3()
        {
            //Arrange
            const string databaseName = "MatchGetAll";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            await SeedData(db);

            var matchService = new MatchService(db.Data, mapper);

            //Act
            var expectedResult = 3;
            var actualResult = await matchService.GetAll();

            //Assert
            Assert.True(expectedResult == actualResult.ToList().Count());
        }

        //[Fact]
        //public async Task AddUmpireShouldAddCurrentUserAsAnUmpire()
        //{
        //}

        private static Task SeedData(FakeSportDbContext db)
        {
            return db.Add(
            new Match
            {
                Id = 1,
                FirstPlayer = new User
                {
                    FirstName = "Player1"
                },
                FirstPlayerId = "1",

                SecondPlayer = new User
                {
                    FirstName = "Player2"
                },
                SecondPlayerId = "2",
                Tournament = new Tournament
                {
                    Id = 1
                },
                IsActive = true
            },
            new Match
            {
                Id = 2,
                FirstPlayer = new User
                {
                    FirstName = "Player3"
                },
                FirstPlayerId = "3",

                SecondPlayer = new User
                {
                    FirstName = "Player4"
                },
                SecondPlayerId = "4",
                Tournament = new Tournament
                {
                    Id = 2
                },
                IsActive = true
            },
            new Match
            {
                Id = 3,
                FirstPlayer = new User
                {
                    FirstName = "Player5"
                },
                FirstPlayerId = "5",

                SecondPlayer = new User
                {
                    FirstName = "Player6"
                },
                SecondPlayerId = "6",
                Tournament = new Tournament
                {
                    Id = 3
                },
                IsActive = false
            });
        }
    }
}
