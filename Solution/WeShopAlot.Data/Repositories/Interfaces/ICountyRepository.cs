using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface ICountyRepository : IAsyncRepository<County>
    {
        County Get(StateProvince stateProvince, string name);

        List<County> GetAll(StateProvince stateProvince);
    }
}
