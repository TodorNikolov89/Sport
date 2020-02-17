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
    using System.Linq;
    using Sport.ViewModels.Tournament;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Collections.Generic;
    using System;

    public class TournamentServiceTests
    {

        [Fact]
        public async Task AllShouldReturnAllTournaments()
        {
            //Arrange
            const string databaseName = "TournamentAll";
            ITournamentService service = await GetTournamentService(databaseName);

            //Act
            var expectedTournamentsCount = 4;

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
            ITournamentService service = await GetTournamentService(databaseName);

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
            ITournamentService service = await GetTournamentService(databaseName);

            //Act
            var tournamentId = 1;
            await service.Delete(tournamentId);

            var tournament = service.ById(tournamentId);

            //Assert
            Assert.True(tournament == null);
        }

        [Fact]
        public async Task EditShouldEditTournamentByModel()
        {
            //Arrange
            const string databaseName = "TournamentEdit";
            ITournamentService service = await GetTournamentService(databaseName);

            //Act
            int tournamentId = 1;
            TournamentFormModel expectedTournament = new TournamentFormModel()
            {
                Id = tournamentId,
                Name = "SofiaOpen",
                Place = "Sofia"
            };

            await service.Edit(expectedTournament);

            var actualTournament = service.ById(tournamentId);

            //Assert
            Assert.Equal(expectedTournament.Name, actualTournament.Name);
            Assert.Equal(expectedTournament.Place, actualTournament.Place);

        }

        [Fact]
        public async Task EditShouldReturnTournamentNotfound()
        {
            //Arrange
            const string databaseName = "TournamentEditNotfound";
            ITournamentService service = await GetTournamentService(databaseName);

            //Act
            int tournamentId = -1;
            TournamentFormModel expectedTournament = new TournamentFormModel()
            {
                Id = tournamentId,
                Name = "SofiaOpen",
                Place = "Sofia"
            };

            await service.Edit(expectedTournament);

            var actualTournament = service.ById(tournamentId);

            //Assert
            Assert.True(actualTournament == null);

        }

        [Fact]
        public async Task SigninShouldSignInCurrentUser()
        {
            //Arrange
            const string databaseName = "TournamentSigninUser";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var mockUserStore = new Mock<IUserStore<User>>();
            UserManager<User> userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);
            //Act
            var user = new User()
            {
                Id = new Guid().ToString()
            };

            mockUserStore.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    Id = user.Id
                });

            service.Signin(1, user);
            var actualResult = db.Data.UserTournament.Any(u => u.UserId == user.Id);

            //Assert
            Assert.True(actualResult);

        }

        [Fact]
        public async Task SignoutShouldSignOutCurrentUser()
        {
            //Arrange
            const string databaseName = "TournamentSignOutUser";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var mockUserStore = new Mock<IUserStore<User>>();
            UserManager<User> userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

            //Act
            var userId = "1";
            var tournamentId = 2;
            await service.Signout(tournamentId, userId);
            var actualResult = db.Data.UserTournament.Any(u => u.UserId == userId);

            //Assert
            Assert.False(actualResult);

        }

        [Fact]
        public async Task GetTournamentPlayersShouldREturnAllPlayers()
        {
            //Arrange
            const string databaseName = "TournamentGetTournamentPlayers";
            ITournamentService service = await GetTournamentService(databaseName);

            var expectedPlayersCount = 8;

            var tournamentId = 3;

            var result = service.GetTournamentPlayers(tournamentId);
            var actualPlayersCount = result.Count();

            //Assert
            Assert.Equal(expectedPlayersCount, actualPlayersCount);
        }

        [Fact]
        public async Task GetDrawPlayersShuoldReturnMatchesForTheSchedule()
        {
            //Arrange
            const string databaseName = "TournamentGetDrawPlayers";
            ITournamentService service = await GetTournamentService(databaseName);

            //Act
            var expectedMatchesCount = 4;
            var tournamentId = 3;
            var result = service.GetDrawPlayers(tournamentId);
            var actualMatchesCount = result.Count();

            //Assert
            Assert.Equal(expectedMatchesCount, actualMatchesCount);
        }

        [Fact]
        public async Task StartShouldCreateMatchesAndSetIsStartedTrue()
        {
            const string databaseName = "TournamentStart";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var mockUserStore = new Mock<IUserStore<User>>();
            UserManager<User> userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

            //Act
            var tournamentId = 4;
            var expectedTournament = db.Data.Tournaments
              .FirstOrDefault(t => t.Id == tournamentId);

            var expectedMatchesCount = 4;

            service.Start(tournamentId);

            var actualTournament = db.Data.Tournaments
                .FirstOrDefault(t => t.Id == tournamentId);

            //Assert
            Assert.True(actualTournament.IsStarted);
            Assert.True(actualTournament.Matches.Count() == expectedMatchesCount);
        }

        [Fact]
        public async Task CreateShouldAddNewTournament()
        {
            const string databaseName = "TournamentCreate";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();


            var mockUserStore = new Mock<IUserStore<User>>();
            var user = new User()
            {
                Id = new Guid().ToString()
            };

            mockUserStore.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    Id = user.Id
                });

            UserManager<User> userManager = GetUserManager(mockUserStore);
            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

            await AddUsers(db);

            //Act
            TournamentFormModel expectedModel = new TournamentFormModel()
            {
                Id = 5,
                Name = "SandanskiOpen",
                StartDate = new DateTime(),
                Place = "Sandanski",
                NumberOfPlayers = 16,
                Type = Domain.Enums.Tournament.TournamentType.Charity
            };

            await service.Create(expectedModel, user.Id);

            var actualModel = service.ById(5);
          
            //Assert
            Assert.True(expectedModel.Id == actualModel.Id);
            Assert.True(expectedModel.Name.Equals(actualModel.Name));
            Assert.True(expectedModel.StartDate == actualModel.StartDate);
            Assert.True(expectedModel.NumberOfPlayers == actualModel.NumberOfPlayers);
            Assert.True(expectedModel.Place.Equals(actualModel.Place));
        }

        private static async Task<ITournamentService> GetTournamentService(string databaseName)
        {
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            if (databaseName.Equals("TournamentCreate")
                || databaseName.Equals("TournamentGetTournamentPlayers"))
            {
                await AddUsers(db);
            }
            else
            {
                await AddTournaments(db);
            }

            var user = new User()
            {
                Id = new Guid().ToString()
            };


            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = GetUserManager(mockUserStore);

            mockUserStore.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    Id = user.Id
                });
            ITournamentService service = new TournamentService(mapper, db.Data, userManager);
            return service;
        }

        private static UserManager<User> GetUserManager(Mock<IUserStore<User>> mockUserStore)
        {
            return new UserManager<User>(mockUserStore.Object, null, null, null, null, null, null, null, null);
        }

        private static Task AddTournaments(FakeSportDbContext db)
        {
            return db.Add(
                new Tournament()
                {
                    Id = 1,
                    Name = "SofiaOpen",
                    Place = "Sofia",
                    NumberOfPlayers = 8
                },
                new Tournament()
                {
                    Id = 2,
                    Name = "BurgasOpen",
                    Place = "Burgas",
                    NumberOfPlayers = 8,
                    Players = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            UserId = "1",
                            TournamentId = 2
                        }
                    }
                },
                new Tournament()
                {
                    Id = 3,
                    Name = "VarnaOpen",
                    Place = "Varna",
                    NumberOfPlayers = 8,
                    Matches = new List<Domain.Match>()
                    {
                        new Domain.Match()
                        {
                            Id = 1
                        },
                         new Domain.Match()
                        {
                            Id = 2
                        },
                          new Domain.Match()
                        {
                            Id = 3
                        },
                           new Domain.Match()
                        {
                            Id = 4
                        }
                    },
                    Players = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            UserId = "2",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "3",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "4",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "5",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "6",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "7",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "8",
                            TournamentId = 3
                        },
                        new UserTournament()
                        {
                            UserId = "9",
                            TournamentId = 3
                        }
                    }
                }
                ,
                new Tournament()
                {
                    Id = 4,
                    Name = "SlivenOpen",
                    Place = "Sliven",
                    NumberOfPlayers = 8,
                    Players = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            User = CreateUser(),
                            UserId = "2",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                            User = CreateUser(),
                            UserId = "3",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                              User = CreateUser(),
                            UserId = "4",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                              User = CreateUser(),
                            UserId = "5",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                              User = CreateUser(),
                            UserId = "6",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                              User = CreateUser(),
                            UserId = "7",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                              User = CreateUser(),
                            UserId = "8",
                            TournamentId = 4
                        },
                        new UserTournament()
                        {
                              User = CreateUser(),
                            UserId = "9",
                            TournamentId = 4
                        }
                    }
                });
        }

        private static User CreateUser()
        {
            return new User();
        }

        private static Task AddUsers(FakeSportDbContext db)
        {
            return db.Add(
                new User()
                {
                    Id = "1",
                    Email = "Test@abv.bg"

                },
                new User()
                {
                    Id = "2",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId="2"
                        }
                    }
                },
                new User()
                {
                    Id = "3",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "3"
                        }
                    }
                },
                new User()
                {
                    Id = "4",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "4"
                        }
                    }
                },
                new User()
                {
                    Id = "5",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "5"
                        }
                    }
                },
                new User()
                {
                    Id = "6",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "6"
                        }
                    }
                },
                new User()
                {
                    Id = "7",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "7"
                        }
                    }
                },
                new User()
                {
                    Id = "8",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "8"
                        }
                    }
                },
                new User()
                {
                    Id = "9",
                    Tournaments = new List<UserTournament>()
                    {
                        new UserTournament()
                        {
                            TournamentId = 3,
                            UserId= "9"
                        }
                    }
                });
        }


    }
}
