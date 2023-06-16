using AutoMapper;
using WeShopAlot.Shared.Services.ServiceAPIClient;
using WeShopAlot.UI.ClientMVC.Services;

namespace WeShopAlot.UI.ClientMVC.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddScoped<IServiceAPIClient, ServiceAPIClient>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
