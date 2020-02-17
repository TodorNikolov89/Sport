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
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

            //Act
            var expectedTournamentsCount = 3;

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

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

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

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

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
            const string databaseName = "TournamentEdit";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddTournaments(db);

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = GetUserManager(mockUserStore);
            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

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

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);

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
            const string databaseName = "TournamentSigninUser";
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();


            await AddTournaments(db);
            //            await AddUsers(db);

            var user = new User()
            {
                Id = new Guid().ToString()
            };

            var mockUserStore = new Mock<IUserStore<User>>();
            UserManager<User> userManager = GetUserManager(mockUserStore);

            ITournamentService service = new TournamentService(mapper, db.Data, userManager);
            mockUserStore.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    Id = user.Id
                });

            //Act
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
            var db = new FakeSportDbContext(databaseName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            var mapper = config.CreateMapper();

            await AddUsers(db);

            var mockUserStore = new Mock<IUserStore<User>>();
            UserManager<User> userManager = GetUserManager(mockUserStore);
            ITournamentService service = new TournamentService(mapper, db.Data, userManager);
           
            var expectedPlayersCount = 8;

            var tournamentId = 3;

            var result = service.GetTournamentPlayers(tournamentId);
            var actualPlayersCount = result.Count();

            //Assert
            Assert.Equal(expectedPlayersCount, actualPlayersCount);
        }

        private static UserManager<User> GetUserManager(Mock<IUserStore<User>> mockUserStore)
        {
            return new UserManager<User>(mockUserStore.Object, null, null, null, null, null, null, null, null);
        }

        private static Tournament GetTournament(FakeSportDbContext db, int tournamentId)
        {
            return db.Data.Tournaments.FirstOrDefault(t => t.Id == tournamentId);
        }

        private static Task AddUserTournament(FakeSportDbContext db)
        {
            return db.Add(
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "2"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "3"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "4"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "5"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "6"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "7"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "8"
               },
               new UserTournament()
               {
                   TournamentId = 3,
                   UserId = "9"
               });
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
                });
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
