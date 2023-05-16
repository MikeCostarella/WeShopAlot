using Microsoft.Extensions.Configuration;
using WeShopAlot.Data;

namespace OurGov.Testing.Shared.Base
{
    internal class BaseIntegrationTestsClass : BaseTestsClass, IDisposable
    {
        #region Member Variables

        protected IConfiguration configuration;
        protected WeShopAlotContext dbContext;

        #endregion Member Variables

        #region Constructors

        public BaseIntegrationTestsClass()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<BaseIntegrationTestsClass>()
                .Build();
        }

        #endregion Constructors

        #region Utilities

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Utilities
    }
}
