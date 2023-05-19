using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderDeliveryMethodsLoadExtensions
    {
        public static void LoadDeliveryMethods(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.DeliveryMethods.Any())
                    {
                        var deliveryMethodsData = File.ReadAllText(path + @"/Content/data/json/deliveryMethods.json");
                        var importedDeliveryMethods = JsonSerializer.Deserialize<List<ImportedDeliveryMethod>>(deliveryMethodsData);
                        foreach (var importedDeliveryMethod in importedDeliveryMethods)
                        {
                            var deliveryMethod = new DeliveryMethod
                            {
                                DeliveryTime = importedDeliveryMethod.DeliveryTime,
                                Description = importedDeliveryMethod.Description,
                                InternalId = importedDeliveryMethod.Id,
                                Price = importedDeliveryMethod.Price,
                                ShortName = importedDeliveryMethod.ShortName
                            };
                            context.DeliveryMethods.Add(deliveryMethod);
                        }
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
