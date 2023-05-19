using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IStateProvinceRepository : IAsyncRepository<StateProvince>
    {
        StateProvince Get(Country country, string name);
    }
}
