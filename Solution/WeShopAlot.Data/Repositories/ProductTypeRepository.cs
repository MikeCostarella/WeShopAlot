using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
    {
        #region Constructors

        public ProductTypeRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public ProductType GetByInternalId(int internalId)
        {
            return dbContext.ProductTypes.FirstOrDefault(x => x.InternalId == internalId);
        }

        #endregion Interface
    }
}
