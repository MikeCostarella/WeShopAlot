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
                            if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            if (!string.IsNullOrEmpty(importedUSOhioCity.Auditor))
                            {
                                var municipalAuditor = new MunicipalAuditor
                                {
                                    FirstName = importedUSOhioCity.AuditorFirstName,
                                    MiddleName = importedUSOhioCity.AuditorMiddleName,
                                    LastName = importedUSOhioCity.AuditorLastName,
                                    MunicipalityId = city.Id
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.AuditorTermEndDate, out termEndDate))
                                {
                                    municipalAuditor.TermEndDate = termEndDate;
                                    municipalAuditor.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalAuditors.Add(municipalAuditor);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.LawDirector))
                            {
                                var municipalLawDirector = new MunicipalLawDirector
                                {
                                    FirstName = importedUSOhioCity.LawDirectorFirstName,
                                    MiddleName = importedUSOhioCity.LawDirectorMiddleName,
                                    LastName = importedUSOhioCity.LawDirectorLastName,
                                    MunicipalityId = city.Id
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.LawDirectorTermEndDate, out termEndDate))
                                {
                                    municipalLawDirector.TermEndDate = termEndDate;
                                    municipalLawDirector.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalLawDirectors.Add(municipalLawDirector);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.Treasurer))
                            {
                                var municipalTreasurer = new MunicipalTreasurer
                                {
                                    FirstName = importedUSOhioCity.TreasurerFirstName,
                                    MiddleName = importedUSOhioCity.TreasurerMiddleName,
                                    LastName = importedUSOhioCity.TreasurerLastName,
                                    MunicipalityId = city.Id
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.TreasurerTermEndDate, out termEndDate))
                                {
                                    municipalTreasurer.TermEndDate = termEndDate;
                                    municipalTreasurer.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalTreasurers.Add(municipalTreasurer);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilPresident))
                            {
                                var municipalCouncilPresident = new MunicipalCouncilPresident
                                {
                                    FirstName = importedUSOhioCity.CouncilPresidentFirstName,
                                    MiddleName = importedUSOhioCity.CouncilPresidentMiddleName,
                                    LastName = importedUSOhioCity.CouncilPresidentLastName,
                                    MunicipalityId = city.Id
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilPresidentTermEndDate, out termEndDate))
                                {
                                    municipalCouncilPresident.TermEndDate = termEndDate;
                                    municipalCouncilPresident.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilPresidents.Add(municipalCouncilPresident);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                        }
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
