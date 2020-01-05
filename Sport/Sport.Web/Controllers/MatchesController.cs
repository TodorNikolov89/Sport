namespace Sport.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Sport.Data;
    using System.Linq;
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

        [Route(nameof(GetAllActive))]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await matchService.GetAllActive();
            return View(result);
        }

        [Route(nameof(ChangeResult))]
        [HttpPost]
        public Task<MatchScoreViewModel> ChangeResult(
            string buttonId,
            int matchId,
            string firstPlayerPoints,
            int firstPlayerGames,
            int firstPlayerSets,
            int firstPlayerTieBreakPoints,
            string secondPlayerPoints,
            int secondPlayerGames,
            int secondPlayerSets,
            int secondPlayerTieBreakPoints)
        {

            var result = matchService.Result(
                buttonId,
                matchId,
                firstPlayerPoints,
                firstPlayerGames,
                firstPlayerSets,
                firstPlayerTieBreakPoints,
                secondPlayerPoints,
                secondPlayerGames,
                secondPlayerSets,
                secondPlayerTieBreakPoints);

            return result;
        }
    }
}