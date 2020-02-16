namespace Sport.Tests
{
    using Profiles;
    using Sport.Services;
    using Sport.Services.Implementation;
    using Sport.Tests.Fakes;

    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;
    using Xunit;
    using System;
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
            var expectedResult = 11;
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
            var invalidMatchId = -1;
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


            var expectedCountOfLiveMatches = 10;
            var actualCountOfLiveMatches = liveMatches.ToList().Count();


            //Assert
            Assert.True(expectedCountOfLiveMatches == actualCountOfLiveMatches,
               $"Expected count of live matches is {expectedCountOfLiveMatches}, not {actualCountOfLiveMatches}!");

        }

        [Fact]
        public async Task AddFirstPlayerPointWhenMatchIsFinishedShouldNotChangeResult()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointWhenMatchIsFinished";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 1);

            var actualResult = await service.AddFirstPlayerPoint(match.Id);

            //Assert
            Assert.True(actualResult.FirstPlayerPoints == 0);
            Assert.True(actualResult.SecondPlayerPoints == 0);

        }

        [Fact]
        public async Task AddFirstPlayerPointWhenResultIsDeuceShouldReturnAdvantage()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointWhenResultIsDeuce";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 4);

            var actualResult = await service.AddFirstPlayerPoint(match.Id);
            var currentGame = actualResult.Sets.FirstOrDefault(s => s.Id == 4)?.Games.Select(g => g.Id).LastOrDefault();

            //Assert
            Assert.True(actualResult.FirstPlayerPoints == 4);
            Assert.True(actualResult.SecondPlayerPoints == 3);
            Assert.True(currentGame == 4);
        }

        [Fact]
        public async Task AddFirstPlayerPointWhenResultIsAdvantageFirstPlayerShouldAddNewGame()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointWhenAdvFP";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 5);
            var firstPlayerGames = match.Sets.Select(s => s.FirstPlayerGames).LastOrDefault();

            var actualMatch = await service.AddFirstPlayerPoint(match.Id);
            var actualSet = actualMatch.Sets.LastOrDefault();

            //Assert
            Assert.True(actualMatch.FirstPlayerPoints == 0);
            Assert.True(actualMatch.SecondPlayerPoints == 0);
            Assert.True(firstPlayerGames + 1 == actualSet.FirstPlayerGames);
        }

        [Fact]
        public async Task AddFirstPlayerPointShouldEndGameAndSet()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointEndGameAndSet";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 6);
            var currentFirstPlayerGames = match.Sets.Select(s => s.FirstPlayerGames).LastOrDefault();
            var currentFirstPlayerSet = match.FirstPlayerSets;

            var actualMatch = await service.AddFirstPlayerPoint(match.Id);
            var actualFirstPlayerGames = actualMatch.Sets.Select(s => s.FirstPlayerGames).FirstOrDefault();
            var actualFirstPlayerSet = actualMatch.FirstPlayerSets;

            //Assert
            Assert.True(actualMatch.FirstPlayerPoints == 0);
            Assert.True(actualMatch.SecondPlayerPoints == 0);
            Assert.True(currentFirstPlayerGames + 1 == actualFirstPlayerGames);
            Assert.True(currentFirstPlayerSet + 1 == actualFirstPlayerSet);
        }

        [Fact]
        public async Task AddFirstPlayerPointShouldEndTheMatch()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointEndsMatch";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 7);
            var expectedMatch = await service.AddFirstPlayerPoint(match.Id);

            //Assert
            Assert.True(expectedMatch.IsFinished, "The match is not finished!");
        }

        [Fact]
        public async Task AddFirstPlayerPointShouldStartTieBreak()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerShouldStartTieBreak";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 8);
            var currentMatch = await service.AddFirstPlayerPoint(match.Id);

            //Assert
            Assert.True(currentMatch.HasTieBreak, "Last set does not have TieBreak!");
        }

        [Fact]
        public async Task AddFirstPlayerPointShouldAddPointSuccesfully()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPoint";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 3);
            var actualResult = await service.AddFirstPlayerPoint(match.Id);

            //Assert
            Assert.True(actualResult.FirstPlayerPoints == 1);
            Assert.True(actualResult.SecondPlayerPoints == 0);

        }

        [Fact]
        public async Task AddFirstPlayerPointShouldAddPointInTieBreak()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointTieBreak";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);

            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 9);
            var currentSet = match.Sets.FirstOrDefault(s => s.HasTieBreak);

            var expectedFirstPlayerTieBreakPoints = 1;
            var expectedSecondPlayerTieBreakPoints = 0;

            var actualResult = await service.AddFirstPlayerPoint(match.Id);
            var actualFirstPlayerTieBreakPoints = actualResult.FirstPlayerTieBreakPoints;
            var actualSecondPlayerTieBreakPoints = actualResult.SecondPlayerTieBreakPoints;


            //Assert
            Assert.Equal(expectedFirstPlayerTieBreakPoints, actualFirstPlayerTieBreakPoints);
            Assert.Equal(expectedSecondPlayerTieBreakPoints, actualSecondPlayerTieBreakPoints);
        }

        [Fact]
        public async Task AddFirstPlayerPointShouldReturnEndTheSetAndAddNewOne()
        {
            //Arrange
            const string databaseName = "MatchAddFirstPlayerPointEndSet";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);
            var expectedFirstPlayerSets = 1;
            var expectedSecondPlayerSets = 0;
            var expectedSetsNumber = 2;
            //Act
            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 10);
            var actualMatch = await service.AddFirstPlayerPoint(match.Id);
            var actualFirstPlayerSets = actualMatch.FirstPlayerSets;
            var actualSecontPlayerSets = actualMatch.SecondPlayerSets;
            var actualSetsNumber = actualMatch.Sets.Count();

            //Assert
            Assert.Equal(expectedFirstPlayerSets, actualFirstPlayerSets);
            Assert.Equal(expectedSecondPlayerSets, actualSecontPlayerSets);
            Assert.Equal(expectedSetsNumber, actualSetsNumber);

        }

        [Fact]
        public async Task AddFirstPlayerPointInTieBreakShouldEndMatch()
        {
            const string databaseName = "MatchAddFirstPlayerInTieBreakEndMatch";
            var db = new FakeSportDbContext(databaseName);
            await SeedData(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            var mapper = config.CreateMapper();

            IMatchService service = new MatchService(db.Data, mapper);
            //Act

            var match = db.Data.Matches.FirstOrDefault(m => m.Id == 11);
            var actualMatch = await service.AddFirstPlayerPoint(match.Id);

            //Assert
            Assert.True(actualMatch.IsFinished);

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
                IsFinished = true,
                Sets = CreateInitialSets(1)
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
                IsFinished = true,
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
                IsActive = false,
                IsFinished = false,
                Sets = CreateInitialSets(3)
            },
            new Domain.Match
            {
                Id = 4,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player7"
                },
                FirstPlayerId = "7",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player8"
                },
                SecondPlayerId = "8",
                Tournament = new Domain.Tournament
                {
                    Id = 4
                },
                IsActive = true,
                IsFinished = false,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=4,
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=4,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=4,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=3
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 5,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "9",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "10",
                Tournament = new Domain.Tournament
                {
                    Id = 5
                },
                IsActive = true,
                IsFinished = false,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=5,
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=5,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=5,
                                        FirstPlayerPoints=4,
                                        SecondPlayerPoints=3
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 6,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "9",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "10",
                Tournament = new Domain.Tournament
                {
                    Id = 6
                },
                IsActive = true,
                IsFinished = false,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=6,
                        FirstPlayerGames=5,
                        SecondPlayerGames=4,
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=6,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=6,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=2
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 7,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "11",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "12",
                Tournament = new Domain.Tournament
                {
                    Id = 7
                },
                IsActive = true,
                IsFinished = false,
                FirstPlayerSets = 1,
                SecondPlayerSets = 0,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=7,
                        FirstPlayerGames=5,
                        SecondPlayerGames=4,
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=7,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=7,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=2
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 8,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "13",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "14",
                Tournament = new Domain.Tournament
                {
                    Id = 8
                },
                IsActive = true,
                IsFinished = false,
                FirstPlayerSets = 1,
                SecondPlayerSets = 0,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=8,
                        FirstPlayerGames=5,
                        SecondPlayerGames=6,
                        HasTieBreak=false,
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=8,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=8,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=2
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 9,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "15",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "16",
                Tournament = new Domain.Tournament
                {
                    Id = 9
                },
                IsActive = true,
                IsFinished = false,
                FirstPlayerSets = 1,
                SecondPlayerSets = 0,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=9,
                        FirstPlayerGames=6,
                        SecondPlayerGames=6,
                        HasTieBreak=true,
                        TieBreak = new Domain.TieBreak()
                        {
                            TieBreakPoints = new List<Domain.TieBreakPoint>
                            {
                                new Domain.TieBreakPoint()
                                {

                                }
                            }
                        },
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=9,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=9,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=2
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 10,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "17",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "18",
                Tournament = new Domain.Tournament
                {
                    Id = 10
                },
                IsActive = true,
                IsFinished = false,
                FirstPlayerSets = 0,
                SecondPlayerSets = 0,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=10,
                        FirstPlayerGames=6,
                        SecondPlayerGames=6,
                        HasTieBreak=true,
                        TieBreak = new Domain.TieBreak()
                        {
                            TieBreakPoints = new List<Domain.TieBreakPoint>
                            {
                                new Domain.TieBreakPoint()
                                {
                                    FirstPlayerPoint=6,
                                    SecondPlayerPoint=5
                                }
                            }
                        },
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=10,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=10,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=2
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Domain.Match
            {
                Id = 11,
                FirstPlayer = new Domain.User
                {
                    FirstName = "Player9"
                },
                FirstPlayerId = "19",

                SecondPlayer = new Domain.User
                {
                    FirstName = "Player10"
                },
                SecondPlayerId = "20",
                Tournament = new Domain.Tournament
                {
                    Id = 11
                },
                IsActive = true,
                IsFinished = false,
                FirstPlayerSets = 1,
                SecondPlayerSets = 0,
                Sets = new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=11,
                        FirstPlayerGames=6,
                        SecondPlayerGames=6,
                        HasTieBreak=true,
                        TieBreak = new Domain.TieBreak()
                        {
                            TieBreakPoints = new List<Domain.TieBreakPoint>
                            {
                                new Domain.TieBreakPoint()
                                {
                                    FirstPlayerPoint=6,
                                    SecondPlayerPoint=5
                                }
                            }
                        },
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=11,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=11,
                                        FirstPlayerPoints=3,
                                        SecondPlayerPoints=2
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private static List<Domain.Set> CreateInitialSets(int id)
        {
            return new List<Domain.Set>()
                {
                    new Domain.Set
                    {
                        Id=id,
                        Games = new List<Domain.Game>()
                        {
                            new Domain.Game()
                            {
                                Id=id,
                                Points = new List<Domain.Point>()
                                {
                                    new Domain.Point()
                                    {
                                        Id=id
                                    }
                                }
                            }
                        }
                    }
                };
        }
    }
}
