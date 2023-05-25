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
            app.LoadCountries();
            app.LoadUSStates();
            app.LoadCanadianProvinces();
            app.LoadUSCounties();
            app.LoadUSOhioTownships();
            app.LoadUSOhioCities();
            app.LoadUSOhioVillages();
            app.LoadUSOhioMunicipalTaxRates();
            app.IndicateMunicipalitiesCollectedByRITA();
            app.LoadProductTypes();
            app.LoadProductBrands();
            app.LoadDeliveryMethods();
            app.LoadProducts();
            //CancellationToken cancellationToken = new CancellationToken();  // ToDo: figure out how to stop the console app in .Net 7
            //hostLifetime.StopAsync(cancellationToken);
        }

        #endregion Initilization Methods
    }
}
