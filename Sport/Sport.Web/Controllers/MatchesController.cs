namespace Sport.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;

    [Route("matches")]
    public class MatchesController : Controller
    {
        private readonly IMatchService matchService;

        public MatchesController(IMatchService matchService)
        {
            this.matchService = matchService;
        }


        [Route(nameof(GetById) + "/{id}")]
        public IActionResult GetById(int id)
        {
            var match = this.matchService.GetMatch(id);

            return View(match);
        }

        [Route(nameof(GetAllActive))]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await matchService.GetAllActive();
            return View(result);
        }

    }
}