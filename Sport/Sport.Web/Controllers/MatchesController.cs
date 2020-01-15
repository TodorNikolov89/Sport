namespace Sport.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Sport.Data;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using ViewModels.Match;

    [Route("matches")]
    public class MatchesController : Controller
    {
        private readonly IMatchService matchService;
        private readonly SportDbContext context;
        private readonly IMapper mapper;

        public MatchesController(IMatchService matchService, SportDbContext context, IMapper mapper)
        {
            this.matchService = matchService;
            this.context = context;
            this.mapper = mapper;
        }


        [Route(nameof(GetById) + "/{id}")]
        public IActionResult GetById(int id)
        {
            var match = this.matchService.GetMatch(id);

            return View(match);
        }

        [Route(nameof(GetAllMatches))]
        public async Task<IActionResult> GetAllMatches()
        {
            var result = await matchService.GetAll();
            return View(result);
        }


        [Route(nameof(ChangeResult))]
        [HttpPost]
        public async Task<LiveResultViewModel> ChangeResult(string buttonId, int matchId)
        {
            LiveResultViewModel result = null;
            if (buttonId.Equals("firstButtonId"))
            {
                result = await matchService.AddFirstPlayerPoint(matchId);
            }

            if (buttonId.Equals("secondButtonId"))
            {
                result = await matchService.AddSecondPlayerPoint(matchId);
            }

            return result;
        }


        [Route(nameof(GetLiveMatches))]
        [HttpPost]
        public IActionResult GetLiveMatches()
        {
            return View();
        }


        [Route(nameof(GetFinishedMatches))]
        [HttpPost]
        public IActionResult GetFinishedMatches()
        {
            return View();
        }


        [Route(nameof(BecomeUmpire) + "/{id}")]
       
        public IActionResult BecomeUmpire(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.matchService.AddUmpire(id, userId);

            return RedirectToAction(nameof(GetAllMatches));
        }
    }
}