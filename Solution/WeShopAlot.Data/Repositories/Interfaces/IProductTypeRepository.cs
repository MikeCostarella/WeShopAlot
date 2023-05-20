using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductTypeRepository : IAsyncRepository<ProductType>
    {
        ProductType GetByInternalId(int internalId);

        ProductType GetByName(string name);
    }
}
