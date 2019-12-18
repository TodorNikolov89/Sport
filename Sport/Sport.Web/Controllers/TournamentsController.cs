namespace Sport.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Sport.Domain;
    using Sport.Services;
    using Sport.ViewModels.Tournament;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Route("tournaments")]
    public class TournamentsController : Controller
    {
        private readonly ITournamentService tournamentService;
        private readonly UserManager<User> userManager;

        public TournamentsController(ITournamentService tournamentService, UserManager<User> userManager)
        {
            this.tournamentService = tournamentService;
            this.userManager = userManager;
        }

        [Route(nameof(All))]
        public IActionResult All()
        {
            var result = tournamentService.All();
            return View(result);
        }


        [Route(nameof(Create))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(Create))]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TournamentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            tournamentService.Create(model);

            return RedirectToAction(nameof(All));
        }

        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id)
        {
            var tournament = this.tournamentService.ById(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }


        [HttpPost]
        [Route(nameof(Edit) + "/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TournamentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.tournamentService.Edit(model);

            return RedirectToAction(nameof(All));
        }

        [Route(nameof(Delete) + "/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return this.View(id);
        }

        [Route(nameof(Destroy) + "/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Destroy(int id)
        {
            await this.tournamentService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [Route(nameof(Signin) + "/{id}")]
        public async Task<IActionResult> Signin(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            this.tournamentService.Signin(id, user);

            return this.View();
        }

        //TODO finish the method
        [Route(nameof(TournamentPlayers) + "/{id}")]
        public async Task<IActionResult> TournamentPlayers(int id)
        {
            var result = this.tournamentService.GetTournamentPlayers(id);

            return this.View(result);
        }

        [Route(nameof(Signout) + "/{id}")]
        public async Task<IActionResult> Signout(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.tournamentService.Signout(id, userId);

            return this.View();
        }
    }
}