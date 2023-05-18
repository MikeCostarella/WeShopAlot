using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        #region Constructors

        public CountryRepository(WeShopAlotContext context) : base(context) { }

        #endregion Constructors

        #region Interface

        public Country GetByAbbreviation(string abbreviation)
        {
            return dbContext.Countries.FirstOrDefault(x => x.Abbreviation == abbreviation);
        }

        #endregion Interface
    }
}
