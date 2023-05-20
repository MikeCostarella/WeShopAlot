using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderOhioTownshipsLoadExtensions
    {
        public static void LoadUSOhioTownships(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Townships.Any())
                    {
                        var countryRepository = new CountryRepository(context);
                        var stateRepository = new StateProvinceRepository(context);
                        var countyRepository = new CountyRepository(context);
                        var country = countryRepository.GetByAbbreviation("US");
                        var state = stateRepository.Get(country, "Ohio");
                        var usOhioTownshipsData = File.ReadAllText(path + @"/Content/data/json/USA-Ohio-townships.json");
                        var importedTownships = JsonSerializer.Deserialize<List<ImportedTownship>>(usOhioTownshipsData);
                        foreach (var importedTownship in importedTownships)
                        {
                            var county = countyRepository.Get(state, importedTownship.County + " County");
                            var township = new Township
                            {
                                Name = importedTownship.County,
                                CountyId = county.Id
                            };
                            context.Townships.Add(township);
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
