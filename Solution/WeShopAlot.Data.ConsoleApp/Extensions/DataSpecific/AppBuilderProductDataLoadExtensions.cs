using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderProductDataLoadExtensions
    {
        public static void LoadProducts(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Products.Any())
                    {
                        var productsData = File.ReadAllText(path + @"/Content/data/json/products.json");
                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                        context.Products.AddRange(products);
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
