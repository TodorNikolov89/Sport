namespace Sport.Services
{
    using Sport.Domain;
    using Sport.ViewModels.Player;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayerService
    {
        Task<IEnumerable<AllPlayersViewModel>> All();

    }
}
