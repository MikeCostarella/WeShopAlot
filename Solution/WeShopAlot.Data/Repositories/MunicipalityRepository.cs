using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class MunicipalityRepository : BaseRepository<Municipality>, IMunicipalityRepository
    {
        #region Constructors

        public MunicipalityRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public Municipality Get(StateProvince stateProvince, string name)
        {
            return dbContext.Municipalities.FirstOrDefault(x => x.StateId == stateProvince.Id && x.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        #endregion Interface
    }
}
