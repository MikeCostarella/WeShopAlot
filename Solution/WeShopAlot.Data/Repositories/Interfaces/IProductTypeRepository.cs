using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductTypeRepository : IGenericRepository<ProductType>
    {
        ProductType GetByInternalId(int internalId);

        ProductType GetByName(string name);
    }
}
