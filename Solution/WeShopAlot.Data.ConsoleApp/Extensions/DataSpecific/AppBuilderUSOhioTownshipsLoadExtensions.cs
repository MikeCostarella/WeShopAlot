using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using WeShopAlot.Data.Import.Models;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories;

namespace WeShopAlot.Data.ConsoleApp.Extensions.DataSpecific
{
    public static class AppBuilderUSOhioTownshipsLoadExtensions
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
                                Name = importedTownship.Name,
                                CountyId = county.Id
                            };
                            context.Townships.Add(township);
                        }
                        if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                        // Load township officials for this year (currently 2022-2023)
                        var townshipRepository = new TownshipRepository(context); 
                        var usOhioTownshipOfficialsData = File.ReadAllText(path + @"/Content/data/json/USA-Ohio-Township-Officials-2022-2023.json");
                        var importedTownshipOfficalsInfos = JsonSerializer.Deserialize<List<ImportedTownshipOfficialsInfo>>(usOhioTownshipOfficialsData);
                        foreach(var importedTownshipOfficalsInfo in importedTownshipOfficalsInfos)
                        {
                            var county = countyRepository.Get(state, importedTownshipOfficalsInfo.CountyName + " County");
                            var township = townshipRepository.Get(county, importedTownshipOfficalsInfo.TownshipName);
                            township.WebSiteUrl = importedTownshipOfficalsInfo.Website;
                            var townshipFiscalOfficer = new TownshipFiscalOfficer
                            {
                                FirstName = importedTownshipOfficalsInfo.FiscalOfficerFirstName,
                                MiddleName = importedTownshipOfficalsInfo.FiscalOfficerMiddleName,
                                LastName = importedTownshipOfficalsInfo.FiscalOfficerLastName,
                                TermEndDate = DateTime.Parse(importedTownshipOfficalsInfo.FiscalOfficerTermEnding),
                                TermStartDate = DateTime.Parse(importedTownshipOfficalsInfo.FiscalOfficerTermEnding).AddYears(-4),
                                TownshipId = township.Id
                            };
                            context.TownshipFiscalOfficers.Add(townshipFiscalOfficer);
                            var townshipTrustee = new TownshipTrustee
                            {
                                FirstName = importedTownshipOfficalsInfo.Trustee1FirstName,
                                MiddleName = importedTownshipOfficalsInfo.Trustee1MiddleName,
                                LastName = importedTownshipOfficalsInfo.Trustee1LastName,
                                TermEndDate = DateTime.Parse(importedTownshipOfficalsInfo.Trustee1TermEnding),
                                TermStartDate = DateTime.Parse(importedTownshipOfficalsInfo.Trustee1TermEnding).AddYears(-4),
                                TownshipId = township.Id
                            };
                            context.TownshipTrustees.Add(townshipTrustee);
                            townshipTrustee = new TownshipTrustee
                            {
                                FirstName = importedTownshipOfficalsInfo.Trustee2FirstName,
                                MiddleName = importedTownshipOfficalsInfo.Trustee2MiddleName,
                                LastName = importedTownshipOfficalsInfo.Trustee2LastName,
                                TermEndDate = DateTime.Parse(importedTownshipOfficalsInfo.Trustee2TermEnding),
                                TermStartDate = DateTime.Parse(importedTownshipOfficalsInfo.Trustee2TermEnding).AddYears(-4),
                                TownshipId = township.Id
                            };
                            context.TownshipTrustees.Add(townshipTrustee);
                            townshipTrustee = new TownshipTrustee
                            {
                                FirstName = importedTownshipOfficalsInfo.Trustee3FirstName,
                                MiddleName = importedTownshipOfficalsInfo.Trustee3MiddleName,
                                LastName = importedTownshipOfficalsInfo.Trustee3LastName,
                                TermEndDate = DateTime.Parse(importedTownshipOfficalsInfo.Trustee3TermEnding),
                                TermStartDate = DateTime.Parse(importedTownshipOfficalsInfo.Trustee3TermEnding).AddYears(-4),
                                TownshipId = township.Id
                            };
                            context.TownshipTrustees.Add(townshipTrustee);
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
