using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeShopAlot.Data.ConsoleApp.Extensions;
using WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific;
using WeShopAlot.Data.Extensions;

namespace WeShopAlot.Data.ConsoleApp
{
    public class Startup
    {
        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion Public Properties

        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Constructors

        #region Initilization Methods

        public void ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables()
                .Build();
            services.AddDbContext<WeShopAlotContext>(options => options.UseSelectedDatabaseServer(configuration));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.MigrateDatabase();
            app.LoadProductTypes();
            app.LoadProductBrands();
            app.LoadDeliveryMethods();
            app.LoadProducts();
        }

        #endregion Initilization Methods
    }
}
