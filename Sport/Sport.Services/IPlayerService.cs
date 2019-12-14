namespace Sport.Services
{
    using Sport.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayerService
    {
        Task<IEnumerable<User>> All();
    }
}
