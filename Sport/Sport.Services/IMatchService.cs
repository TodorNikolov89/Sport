namespace Sport.Services
{
    using Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMatchService
    {
        Match GetMatch(int id);

        Task<IEnumerable<Match>> GetAllActive();
    }
}
