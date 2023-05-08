using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        #region Constructors

        public ProductRepository(MeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
