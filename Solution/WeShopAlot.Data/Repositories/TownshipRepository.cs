using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class TownshipRepository : BaseRepository<Township>, ITownshipRepository
    {
        #region Constructors

        public TownshipRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public Township Get(County county, string name)
        {
            return dbContext.Townships.FirstOrDefault(x => x.CountyId == county.Id && x.Name.Trim() == name.Trim());
        }

        public List<Township> GetAll(StateProvince stateProvince)
        {
            // ToDo: need to join to county
            return dbContext.Townships.ToList();
        }

        #endregion Interface
    }
}
