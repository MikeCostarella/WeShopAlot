using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories
{
    public class ProductBrandRepository : BaseRepository<ProductType>
    {
        #region Constructors

        public ProductBrandRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
