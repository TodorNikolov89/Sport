namespace Sport.Web.Hubs
{
    using Services;

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class SportHub : Hub
    {
        private readonly IMatchService matchService;

        public SportHub(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        public async Task UpdateResult(int id)
        {
            var match = matchService.GetMatch(id);

            await Clients.All.SendAsync("ReceiveResult", match);
        }
    }
}
