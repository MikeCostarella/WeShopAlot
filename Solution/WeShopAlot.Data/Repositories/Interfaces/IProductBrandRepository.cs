using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductBrandRepository : IGenericRepository<ProductBrand>
    {
        ProductBrand GetByInternalId(int internalId);

        ProductBrand GetByName(string name);
    }
}
