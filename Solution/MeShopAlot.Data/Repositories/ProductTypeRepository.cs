using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories
{
    public class ProductTypeRepository : BaseRepository<ProductType>
    {
        #region Constructors

        public ProductTypeRepository(MeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
