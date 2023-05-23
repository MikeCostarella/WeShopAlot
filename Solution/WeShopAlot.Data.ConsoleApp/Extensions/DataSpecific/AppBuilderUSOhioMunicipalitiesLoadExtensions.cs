using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;
using WeShopAlot.Shared.Enumerations;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderUSOhioMunicipalitiesLoadExtensions
    {
        public static void LoadUSOhioMunicipalities(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Municipalities.Any(x => x.MunicipalityTypeId == (int)MunicipalityTypeEnum.City))
                    {
                        var countryRepository = new CountryRepository(context);
                        var country = countryRepository.GetByAbbreviation("US");
                        var stateRepository = new StateProvinceRepository(context);
                        var state = stateRepository.Get(country, "Ohio");
                        var usOhioCitiesData = File.ReadAllText(path + @"/Content/data/json/USA-Ohio-City-Officials-2022-2023.json");
                        var importedUSOhioCities = JsonSerializer.Deserialize<List<ImportedCity>>(usOhioCitiesData);
                        foreach (var importedUSOhioCity in importedUSOhioCities)
                        {
                            var city = new Municipality
                            {
                                Census2000 = importedUSOhioCity.Census2000,
                                Census2010 = importedUSOhioCity.Census2010,
                                Census2020 = importedUSOhioCity.Census2020,
                                FormOfGovernment = importedUSOhioCity.FormOfGovernment,
                                MailingAddressLine1 = importedUSOhioCity.Address1,
                                MailingAddressLine2 = importedUSOhioCity.Address2,
                                MunicipalityTypeId = (int)MunicipalityTypeEnum.City,
                                Name = importedUSOhioCity.CityName,
                                StateId = state.Id,
                                Telephone = importedUSOhioCity.Telephone,
                                Website = importedUSOhioCity.Website,
                                YearIncorporated = importedUSOhioCity.YearIncorporated,
                                ZIPCode = importedUSOhioCity.ZIPCode
                            };
                            context.Municipalities.Add(city);
                            if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            var mayor = new Mayor
                            {
                                FirstName = importedUSOhioCity.MayorFirstName,
                                MiddleName = importedUSOhioCity.MayorMiddleName,
                                LastName = importedUSOhioCity.MayorLastName,
                                MunicipalityId = city.Id
                            };
                            DateTime termEndDate = DateTime.MinValue;
                            if (DateTime.TryParse(importedUSOhioCity.MayorTermEndDate, out termEndDate))
                            {
                                mayor.TermEndDate = termEndDate;
                                mayor.TermStartDate = termEndDate.AddYears(-4);
                            }
                            context.Mayors.Add(mayor);
                        }
                        if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
