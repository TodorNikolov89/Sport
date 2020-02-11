namespace Sport.Tests
{
    using Profiles;
    using Domain;
    using Data;
    using Services.Implementation;

    using System.Threading.Tasks;
    using Xunit;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    public class MatchServiceTests
    {
        [Fact]
        public async Task GetMatchTestShouldReturnTrueIfMatchExists()
        {
            //Arrange

            var options = new DbContextOptionsBuilder<SportDbContext>()
                .UseInMemoryDatabase("MatchGetMatch")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();
                       
            await using (var initialDbContext = new SportDbContext(options))
            {
                initialDbContext.Matches.AddRange(new Match
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

                await initialDbContext.SaveChangesAsync();
            }

            await using var dbContext = new SportDbContext(options);
            var matchService = new MatchService(dbContext, mapper);

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
