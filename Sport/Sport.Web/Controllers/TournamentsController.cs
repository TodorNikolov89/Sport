namespace Sport.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Sport.Services;
    using Sport.ViewModels.Tournament;
    using System.Threading.Tasks;

    [Route("tournaments")]
    public class TournamentsController : Controller
    {
        private readonly ITournamentService tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
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
    }
}