using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderCountryLoadExtensions
    {
        public static void LoadCountries(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Countries.Any())
                    {
                        var countriesData = File.ReadAllText(path + @"/Content/data/json/countries.json");
                        var importedCountries = JsonSerializer.Deserialize<List<ImportedCountry>>(countriesData);
                        foreach (var importedCountry in importedCountries)
                        {
                            var country = new Country
                            {
                                Abbreviation = importedCountry.Code,
                                Name = importedCountry.Name
                            };
                            context.Countries.Add(country);
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
