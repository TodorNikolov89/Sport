namespace Sport.Web.Controllers
{
    using Services;

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [Route("players")]
    public class PlayersController : Controller
    {
        private readonly IPlayerService playerService;

        public PlayersController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        [Route(nameof(All))]
        public async Task<IActionResult> All()
        {
            var result = await playerService.All();
            return View(result);
        }
    }
}