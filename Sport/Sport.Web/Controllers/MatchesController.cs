﻿namespace Sport.Web.Controllers
{
    using Data;
    using Hubs;
    using Services;
    using ViewModels.Match;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;    
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Route("matches")]
    public class MatchesController : Controller
    {
        private readonly IHubContext<SportHub> sportHub;
        private readonly IMatchService matchService;
        private readonly SportDbContext context;
        private readonly IMapper mapper;

        public MatchesController(IHubContext<SportHub> sportHub, IMatchService matchService, SportDbContext context, IMapper mapper)
        {
            this.sportHub = sportHub;
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

            await this.sportHub.Clients.All.SendAsync("ReceiveResult", result);

            return result;
        }


        [Route(nameof(GetLiveMatches))]
        public async Task<IActionResult> GetLiveMatches()
        {
            var result = await this.matchService.GetLiveMatches();

            return View(result);
        }


        [Route(nameof(GetFinishedMatches))]
        public async Task<IActionResult> GetFinishedMatches()
        {
            var result = await this.matchService.GetFinishedMatches();

            return View(result);
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