using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Testing.Repositories
{
    [TestClass]
    public class StateProvinceRepositoryTests
    {
        [TestMethod]
        public void TestGetStateOfCountry()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<WeShopAlotContext>();
            dbContextOptionsBuilder.UseSqlServer("Server=localhost;Database=WeShopAlot;User Id=WeShopAlot; Password=WeShopAlot;Trusted_Connection=true;TrustServerCertificate=true;");
            var dbContext = new WeShopAlotContext(dbContextOptionsBuilder.Options);
            var countryRepository = new CountryRepository(dbContext);
            var country = countryRepository.GetByAbbreviation("CA");
            var stateRepository = new StateProvinceRepository(dbContext);
            var state = stateRepository.Get(country, "Alberta");
            Assert.IsTrue(state != null);
        }
    }
}
