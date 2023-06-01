using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class ProductBrandRepository : GenericRepository<ProductBrand>, IProductBrandRepository
    {
        #region Constructors

        public ProductBrandRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public ProductBrand GetByInternalId(int internalId)
        {
            return dbContext.ProductBrands.FirstOrDefault(x => x.InternalId == internalId);
        }

        public ProductBrand GetByName(string name)
        {
            return dbContext.ProductBrands.FirstOrDefault(x => x.Name.Trim() == name.Trim());
        }

        #endregion Interface
    }
}
