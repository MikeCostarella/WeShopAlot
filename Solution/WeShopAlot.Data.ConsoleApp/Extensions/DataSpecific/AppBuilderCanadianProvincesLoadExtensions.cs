using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderCanadianProvincesLoadExtensions
    {
        public static void LoadCanadianProvinces(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var countryRepository = new CountryRepository(context);
                    var country = countryRepository.GetByAbbreviation("CA");
                    if (!context.StateProvinces.Any(x => x.CountryId == country.Id))
                    {
                        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        var canadianProvincesData = File.ReadAllText(path + @"/Content/data/json/Canada-provinces.json");
                        var importedCanadianProvinces = JsonSerializer.Deserialize<List<ImportedCanadianProvince>>(canadianProvincesData);
                        foreach (var importedCanadianProvince in importedCanadianProvinces)
                        {
                            var stateProvince = new StateProvince
                            {
                                Abbreviation = importedCanadianProvince.abbreviation,
                                CountryId = country.Id,
                                Name = importedCanadianProvince.name
                            };
                            context.StateProvinces.Add(stateProvince);
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
