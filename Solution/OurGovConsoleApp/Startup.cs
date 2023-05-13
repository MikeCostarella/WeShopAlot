using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MeShopAlot.Data.ConsoleApp
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
            string connectionstring = configuration.GetConnectionString("MeShopAlotConnection");
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new InvalidOperationException("No db connection specified");
            }
            services.AddDbContext<MeShopAlotContext>(options => options.UseSqlServer(connectionstring));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.MigrateDatabase();
            CancellationToken cancellationToken = new CancellationToken();
            //hostLifetime.StopAsync(cancellationToken);
        }

        #endregion Initilization Methods
    }
}
