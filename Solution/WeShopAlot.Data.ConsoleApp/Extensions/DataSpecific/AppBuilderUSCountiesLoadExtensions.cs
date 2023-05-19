using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderUSCountiesLoadExtensions
    {
        public static void LoadUSCounties(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Counties.Any())
                    {
                        var countryRepository = new CountryRepository(context);
                        var country = countryRepository.GetByAbbreviation("US");
                        var stateRepository = new StateProvinceRepository(context);
                        var usCountiesData = File.ReadAllText(path + @"/Content/data/json/USA-counties.json");
                        var importedCounties = JsonSerializer.Deserialize<List<ImportedCounty>>(usCountiesData);
                         foreach (var importedCounty in importedCounties)
                        {
                            var state = stateRepository.Get(country, importedCounty.State);
                            var county = new County
                            {
                                Name = importedCounty.County,
                                StateProvinceId = state.Id
                            };
                            context.Counties.Add(county);
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
