using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class ProductBrandRepository : BaseRepository<ProductBrand>, IProductBrandRepository
    {
        #region Constructors

        public ProductBrandRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public ProductBrand GetByInternalId(int internalId)
        {
            return dbContext.ProductBrands.FirstOrDefault(x => x.InternalId == internalId);
        }

        #endregion Interface
    }
}
