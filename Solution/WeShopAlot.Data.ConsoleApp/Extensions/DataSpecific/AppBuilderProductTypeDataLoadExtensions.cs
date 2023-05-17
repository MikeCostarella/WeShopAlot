using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderProductTypeDataLoadExtensions
    {
        public static void LoadProductTypes(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.ProductTypes.Any())
                    {
                        var productTypesData = File.ReadAllText(path + @"/Content/data/json/producttypes.json");
                        var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                        context.ProductTypes.AddRange(productTypes);
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
