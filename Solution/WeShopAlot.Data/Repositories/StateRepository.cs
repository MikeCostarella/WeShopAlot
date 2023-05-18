using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        #region Constructors

        public StateRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public State Get(Country country, string name)
        {
            return dbContext.States.FirstOrDefault(x => x.CountryId == country.Id && x.Name == name);
        }

        #endregion Interface
    }
}
