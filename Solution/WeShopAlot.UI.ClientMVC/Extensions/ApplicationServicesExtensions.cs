using WeShopAlot.Shared.Services.ServiceAPIClient;
using WeShopAlot.UI.ClientMVC.Services;

namespace WeShopAlot.UI.ClientMVC.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // AutoMapper 15+: register through DI with the license key (see the WebAPI project).
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = config["AutoMapper:LicenseKey"];
                cfg.AddProfile<MappingProfile>();
            });
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddScoped<IServiceAPIClient, ServiceAPIClient>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
