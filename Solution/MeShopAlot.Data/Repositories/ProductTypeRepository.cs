using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Data.Repositories
{
    public class ProductTypeRepository : BaseRepository<ProductType>
    {
        #region Constructors

        public ProductTypeRepository(MeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
