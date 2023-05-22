using WeShopAlot.Data.Interfaces;
using WeShopAlot.Data.Repositories.Interfaces;
using WeShopAlot.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using WeShopAlot.Infrastructure.Services;
using WeShopAlot.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WeShopAlot.Infrastructure.Services.Interfaces;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.WebAPI.Errors;

namespace WeShopAlot.WebAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddDbContext<WeShopAlotContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("WeShopAlotSQLConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductBrandRepository, ProductBrandRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            return services;
        }
    }
}
