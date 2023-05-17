using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.ConsoleApp.Extensions;
using WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific;

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
            string connectionstring = configuration.GetConnectionString("WeShopAlotConnection");
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new InvalidOperationException("No db connection specified");
            }
            services.AddDbContext<WeShopAlotContext>(options => options.UseSqlServer(connectionstring));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.MigrateDatabase();
            app.LoadProductTypes();
            app.LoadProductBrands();
            app.LoadProducts();
            //CancellationToken cancellationToken = new CancellationToken();
            //hostLifetime.StopAsync(cancellationToken);
        }

        #endregion Initilization Methods
    }
}
