using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sport.Data;
using Sport.Domain;
using Sport.ViewModels.Player;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Services.Implementation
{
    public class PlayerService : IPlayerService
    {
        private readonly IMapper mapper;
        private readonly SportDbContext context;
        private readonly UserManager<User> userManager;

        public PlayerService(IMapper mapper, SportDbContext context, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<AllPlayersViewModel>> All()
        {
            var allPlayers = await userManager.GetUsersInRoleAsync("Player");

            var result = mapper.Map<IEnumerable<AllPlayersViewModel>>(allPlayers);

            return result;
        }

       
    }
}
