using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.ConsoleApp.Extensions
{
    public static class AppBuilderDatabaseExtensions
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    context.Database.ExecuteSqlRaw(File.ReadAllText(dir + @"\Scripts\DropDatabase.sql"));
                }
                catch (Exception ex)
                {
                    throw;
                }
                context.Database.Migrate();
            }
        }

        public static void LoadProductData(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    //if (!context.ProductBrands.Any())
                    //{
                    //    var brandsData = File.ReadAllText(path + @"/Content/data/json/brands.json");
                    //    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    //    context.ProductBrands.AddRange(brands);
                    //}
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
