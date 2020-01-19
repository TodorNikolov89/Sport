﻿namespace Sport.Services
{
    using Domain;
    using ViewModels.Match;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMatchService
    {
        LiveResultViewModel GetMatch(int id);

        Task<IEnumerable<Match>> GetAll();

        Task<LiveResultViewModel> AddFirstPlayerPoint(int matchId);

        Task<LiveResultViewModel> AddSecondPlayerPoint(int matchId);


        void AddUmpire(int id, string userId);
    }
}
