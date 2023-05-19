using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class StateProvinceRepository : BaseRepository<StateProvince>, IStateProvinceRepository
    {
        #region Constructors

        public StateProvinceRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public StateProvince Get(Country country, string name)
        {
            return dbContext.StateProvinces.FirstOrDefault(x => x.CountryId == country.Id && x.Name == name);
        }

        #endregion Interface
    }
}
