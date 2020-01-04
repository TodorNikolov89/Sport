namespace Sport.Services.Implementation
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data;
    using Domain;
    using ViewModels.Player;

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
