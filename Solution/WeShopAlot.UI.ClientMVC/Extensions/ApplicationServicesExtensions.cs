using WeShopAlot.UI.ClientMVC.Services;

namespace WeShopAlot.UI.ClientMVC.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
