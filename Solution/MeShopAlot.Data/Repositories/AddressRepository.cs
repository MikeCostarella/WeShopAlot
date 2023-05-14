using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Base;

namespace MeShopAlot.Data.Repositories
{
    public class AddressRepository : BaseRepository<Address>
    {
        #region Constructors

        public AddressRepository(MeShopAlotContext context) : base(context) { }

        #endregion Constructors
    }
}
