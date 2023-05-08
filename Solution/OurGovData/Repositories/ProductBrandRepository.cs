using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Data.Repositories
{
    public class ProductBrandRepository : BaseRepository<ProductType>
    {
        #region Constructors

        public ProductBrandRepository(MeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
