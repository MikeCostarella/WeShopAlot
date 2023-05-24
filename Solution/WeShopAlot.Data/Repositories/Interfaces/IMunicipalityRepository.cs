using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IMunicipalityRepository : IAsyncRepository<Municipality>
    {
        Municipality Get(StateProvince stateProvince, string name);
    }
}
