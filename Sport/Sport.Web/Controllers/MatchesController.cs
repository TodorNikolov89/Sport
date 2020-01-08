﻿namespace Sport.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Sport.Data;
    using Sport.Domain;
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
        public async Task<MatchScoreViewModel> ChangeResult(string buttonId, int matchId)
        {
            var result = await matchService.Result(buttonId, matchId);

            return result;
        }
    }
}