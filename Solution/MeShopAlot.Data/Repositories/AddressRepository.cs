using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories
{
    public class AddressRepository : BaseRepository<Address>
    {
        #region Constructors

        public AddressRepository(MeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
