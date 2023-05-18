using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderStateLoadExtensions
    {
        public static void LoadUSStates(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.States.Any())
                    {
                        var countryRepository = new CountryRepository(context);
                        var country = countryRepository.GetByAbbreviation("US");
                        var usStatesData = File.ReadAllText(path + @"/Content/data/json/USA-states.json");
                        var importedStates = JsonSerializer.Deserialize<List<ImportedState>>(usStatesData);
                        foreach (var importedState in importedStates)
                        {
                            var state = new State
                            {
                                Abbreviation = importedState.code,
                                CountryId = country.Id,
                                Name = importedState.name
                            };
                            context.States.Add(state);
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
