using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IStateRepository : IAsyncRepository<State>
    {
        State Get(Country country, string name);
    }
}
