using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface ITownshipRepository : IAsyncRepository<Township>
    {
        Township Get(County county, string name);

        List<Township> GetAll(StateProvince stateProvince);
    }
}
