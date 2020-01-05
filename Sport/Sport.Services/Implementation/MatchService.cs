namespace Sport.Services.Implementation
{
    using Data;
    using Domain;

    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Sport.ViewModels.Match;

    public class MatchService : IMatchService
    {
        private readonly SportDbContext context;
        private readonly IMapper mapper;

        public MatchService(SportDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Match>> GetAllActive()
        {
            var matches = await this.context
                .Matches
                .Where(m => m.IsActive)
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .ToListAsync();

            return matches;
        }

        public MatchScoreViewModel GetMatch(int id)
        {
            var dbMatch = this.context
                .Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .FirstOrDefault(m => m.Id == id);

            var match = mapper.Map<MatchScoreViewModel>(dbMatch);

            return match;
        }

        public MatchScoreViewModel Result(string buttonId, int matchId, string firstPlayerPoints, int firstPlayerGames, int firstPlayerSets, int firstPlayerTieBreakPoints, string secondPlayerPoints, int secondPlayerGames, int secondPlayerSets, int secondPlayerTieBreakPoints)
        {
            throw new System.NotImplementedException();
        }

        public MatchScoreViewModel Result(MatchScoreViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
