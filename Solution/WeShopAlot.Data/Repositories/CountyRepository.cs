using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class CountyRepository : BaseRepository<County>, ICountyRepository
    {
        #region Constructors

        public CountyRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public County Get(StateProvince stateProvince, string name)
        {
            return dbContext.Counties.FirstOrDefault(x => x.StateProvinceId == stateProvince.Id && x.Name.Trim() == name.Trim());
        }

        #endregion Interface
    }
}
