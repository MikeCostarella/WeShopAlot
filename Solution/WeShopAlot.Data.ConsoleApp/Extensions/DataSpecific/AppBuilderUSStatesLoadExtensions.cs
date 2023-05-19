using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderUSStatesLoadExtensions
    {
        public static void LoadUSStates(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var countryRepository = new CountryRepository(context);
                    var country = countryRepository.GetByAbbreviation("US");
                    if (!context.StateProvinces.Any(x => x.CountryId == country.Id))
                    {
                        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        var usStatesData = File.ReadAllText(path + @"/Content/data/json/USA-states.json");
                        var importedStates = JsonSerializer.Deserialize<List<ImportedState>>(usStatesData);
                        foreach (var importedState in importedStates)
                        {
                            var state = new StateProvince
                            {
                                Abbreviation = importedState.code,
                                CountryId = country.Id,
                                Name = importedState.name
                            };
                            context.StateProvinces.Add(state);
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
