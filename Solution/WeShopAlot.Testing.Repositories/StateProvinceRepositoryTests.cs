using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeShopAlot.Data;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Testing.Repositories
{
    [TestClass]
    public class StateProvinceRepositoryTests
    {
        private IConfiguration Configuration { get; }
        private string connectionString { get; }

        public StateProvinceRepositoryTests() {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<StateProvinceRepositoryTests>();
            Configuration = builder.Build();
            connectionString = Configuration.GetConnectionString("WeShopAlotSQLConnection");
        }

        [TestMethod]
        public void TestGetStateOfCountry()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<WeShopAlotContext>();
            dbContextOptionsBuilder.UseSqlServer(connectionString);
            var dbContext = new WeShopAlotContext(dbContextOptionsBuilder.Options);
            var countryRepository = new CountryRepository(dbContext);
            var country = countryRepository.GetByAbbreviation("CA");
            var stateRepository = new StateProvinceRepository(dbContext);
            var state = stateRepository.Get(country, "Alberta");
            Assert.IsTrue(state != null);
        }
    }
}
