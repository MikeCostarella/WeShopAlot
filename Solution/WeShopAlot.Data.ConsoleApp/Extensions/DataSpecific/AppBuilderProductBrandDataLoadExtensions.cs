using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderProductBrandDataLoadExtensions
    {
        public static void LoadProductBrands(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.ProductBrands.Any())
                    {
                        var productBrandsData = File.ReadAllText(path + @"/Content/data/json/productbrands.json");
                        var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandsData);
                        context.ProductBrands.AddRange(productBrands);
                    }
                    if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
