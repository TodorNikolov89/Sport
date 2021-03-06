﻿namespace Sport.Services
{
    using ViewModels.Match;

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sport.Domain;

    public interface IMatchService
    {
        LiveResultViewModel GetMatch(int id);

        Task<IEnumerable<AllMatchesViewModel>> GetAll();

        Task<LiveResultViewModel> AddFirstPlayerPoint(int matchId);

        Task<LiveResultViewModel> AddSecondPlayerPoint(int matchId);

        void AddUmpire(int id, string userId);

        Task<IEnumerable<LiveMatchesViewModel>> GetLiveMatches();

        Task<IEnumerable<FinishedMatchesViewModel>> GetFinishedMatches();

        Task<Match> GetCurentMatchDetais(int matchId);

    }
}