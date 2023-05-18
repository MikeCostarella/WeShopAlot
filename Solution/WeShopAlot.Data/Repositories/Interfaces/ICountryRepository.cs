using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface ICountryRepository : IAsyncRepository<Country>
    {
        Country GetByAbbreviation(string abbreviation);
    }
}
