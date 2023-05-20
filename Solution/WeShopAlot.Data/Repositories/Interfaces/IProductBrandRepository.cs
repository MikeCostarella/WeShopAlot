using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IProductBrandRepository : IAsyncRepository<ProductBrand>
    {
        ProductBrand GetByInternalId(int internalId);

        ProductBrand GetByName(string name);
    }
}
