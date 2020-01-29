namespace Sport.Web.Hubs
{
    using Sport.ViewModels.Match;

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    public class SportHub : Hub
    {
        public async Task UpdateResult(LiveResultViewModel match)
        {
            await Clients.All.SendAsync("ReceiveResult", match);
        }
    }
}
