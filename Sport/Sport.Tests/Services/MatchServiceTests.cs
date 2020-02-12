namespace Sport.Tests
{
    using Profiles;
    using Domain;
    using Data;
    using Services.Implementation;

    using System.Threading.Tasks;
    using Xunit;
    using AutoMapper;
    using Sport.Tests.Fakes;

    public class MatchServiceTests
    {
        [Fact]
        public async Task GetMatchTestShouldReturnTrueIfMatchExists()
        {
            //Arrange
            const string databaseName = "MatchGetMatch";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();


            await db.Add(new Match
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
                }
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
                 }
             });

            var matchService = new MatchService(db.Data, mapper);

            //Act
            var expectedResult = 2;
            var actualResult = matchService.GetMatch(expectedResult);

            //Assert
            Assert.True(expectedResult == actualResult.Id);

        }


        //public Task GetAllTest()
        //{
        //    var options = new DbContextOptionsBuilder<SportDbContext>()
        //       .UseInMemoryDatabase("MatchGetAll")
        //       .Options;
        //}
    }
}
