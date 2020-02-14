﻿namespace Sport.Tests
{
    using Profiles;
    using Sport.Services.Implementation;
    using Sport.Tests.Fakes;

    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;
    using Xunit;
    using Sport.Services;
    using System;
    using Moq;
    using Sport.ViewModels.Match;
    using System.Collections.Generic;

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

        [Fact]
        public async Task AddUmpireShouldReturnMatchIsNotFound()
        {
            //Arrange
            const string databaseName = "MatchAddUmpire";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            IMatchService service = new MatchService(db.Data);

            //Act

            var invalidMatchId = 5;
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == invalidMatchId);
            var user = db.Data.Users.FirstOrDefault(u => u.FirstName == "Player1");

            //Assert
            Assert.Throws<NullReferenceException>(() => service.AddUmpire(match.Id, user.Id));
        }

        [Fact]
        public async Task GetFinishedMatchesShouldReturnAllFinishedMatches()
        {
            //Arrange
            const string databaseName = "MatchGetFinishedMatches";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var finishedMatches = await service.GetFinishedMatches();

            var expectedCountOfFinishedMatches = 2;
            var actualCountOfFinishedMatches = finishedMatches.ToList().Count();

            //Assert
            Assert.True(expectedCountOfFinishedMatches == actualCountOfFinishedMatches,
                $"Expected count of finished mathces is {expectedCountOfFinishedMatches}, not {actualCountOfFinishedMatches}!");


        }


        [Fact]
        public async Task GetLiveMatchesShouldReturnAllLiveMatches()
        {
            //Arrange
            const string databaseName = "MatchGetAllLiveMatches";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act

            var liveMatches = await service.GetLiveMatches();


            var expectedCountOfLiveMatches = 2;
            var actualCountOfLiveMatches = liveMatches.ToList().Count();


            //Assert
            Assert.True(expectedCountOfLiveMatches == actualCountOfLiveMatches,
               $"Expected count of live matches is {expectedCountOfLiveMatches}, not {actualCountOfLiveMatches}!");

        }


        private static Task SeedData(FakeSportDbContext db)
        {
            return db.Add(
            new Domain.Match
            {
                Id = 1,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player1"
                },
                FirstPlayerId = "1",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player2"
                },
                SecondPlayerId = "2",
                Tournament = new Domain.Tournament
                {
                    Id = 1
                },
                IsActive = true,
                IsFinished = true
            },
            new Domain.Match
            {
                Id = 2,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player3"
                },
                FirstPlayerId = "3",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player4"
                },
                SecondPlayerId = "4",
                Tournament = new Domain.Tournament
                {
                    Id = 2
                },
                IsActive = true,
                IsFinished = true
            },
            new Domain.Match
            {
                Id = 3,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player5"
                },
                FirstPlayerId = "5",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player6"
                },
                SecondPlayerId = "6",
                Tournament = new Domain.Tournament
                {
                    Id = 3
                },
                IsActive = false
            });
        }
    }
}