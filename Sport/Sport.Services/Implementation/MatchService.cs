namespace Sport.Services.Implementation
{
    using Data;
    using Sport.Domain;
    using System.Collections.Generic;
    //using System.Data.Entity;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class MatchService : IMatchService
    {
        private readonly SportDbContext context;

        public MatchService(SportDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Match>> GetAllActive()
        {
            var matches = await this.context
                .Matches
                .Where(m => m.IsActive)
                .Include(m=>m.FirstPlayer)
                .Include(m=>m.SecondPlayer)
                .Include(m=>m.Tournament)
                .ToListAsync();

            return matches;
        }

        public Match GetMatch(int id)
        {
            var match = this.context
                .Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .Include(m => m.Tournament)
                .FirstOrDefault(m => m.Id == id);

            return match;
        }
    }
}
