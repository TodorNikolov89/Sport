namespace Sport.Services.Implementation
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data;
    using Domain;
    using ViewModels.Player;
    using Sport.Services.Paging;
    using System.Linq;

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
           // var querableResult = result.AsQueryable();
           // var paginatedResult = await PaginatedList<AllPlayersViewModel>.CreateAsync(querableResult, 1, 3);

            return result;
        }
    }
}
