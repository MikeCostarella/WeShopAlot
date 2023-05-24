using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
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
        public static void LoadUSOhioCities(this IApplicationBuilder app)
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
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember01Title))
                            {
                                var municipalCouncilMember01 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember01FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember01MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember01LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember01Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember01TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember01.TermEndDate = termEndDate;
                                    municipalCouncilMember01.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember01);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember02Title))
                            {
                                var municipalCouncilMember02 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember02FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember02MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember02LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember02Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember02TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember02.TermEndDate = termEndDate;
                                    municipalCouncilMember02.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember02);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember03Title))
                            {
                                var municipalCouncilMember03 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember03FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember03MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember03LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember03Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember03TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember03.TermEndDate = termEndDate;
                                    municipalCouncilMember03.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember03);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember04Title))
                            {
                                var municipalCouncilMember04 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember04FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember04MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember04LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember04Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember04TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember04.TermEndDate = termEndDate;
                                    municipalCouncilMember04.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember04);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember05Title))
                            {
                                var municipalCouncilMember05 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember05FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember05MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember05LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember05Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember05TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember05.TermEndDate = termEndDate;
                                    municipalCouncilMember05.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember05);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember06Title))
                            {
                                var municipalCouncilMember06 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember06FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember06MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember06LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember06Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember06TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember06.TermEndDate = termEndDate;
                                    municipalCouncilMember06.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember06);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember07Title))
                            {
                                var municipalCouncilMember07 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember07FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember07MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember07LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember07Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember07TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember07.TermEndDate = termEndDate;
                                    municipalCouncilMember07.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember07);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember08Title))
                            {
                                var municipalCouncilMember08 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember08FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember08MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember08LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember08Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember08TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember08.TermEndDate = termEndDate;
                                    municipalCouncilMember08.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember08);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember09Title))
                            {
                                var municipalCouncilMember09 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember09FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember09MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember09LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember09Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember09TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember09.TermEndDate = termEndDate;
                                    municipalCouncilMember09.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember09);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember10Title))
                            {
                                var municipalCouncilMember10 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember10FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember10MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember10LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember10Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember10TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember10.TermEndDate = termEndDate;
                                    municipalCouncilMember10.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember10);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember11Title))
                            {
                                var municipalCouncilMember11 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember11FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember11MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember11LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember11Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember11TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember11.TermEndDate = termEndDate;
                                    municipalCouncilMember11.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember11);
                                if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                            }
                            if (!string.IsNullOrEmpty(importedUSOhioCity.CouncilMember12Title))
                            {
                                var municipalCouncilMember12 = new MunicipalCouncilMember
                                {
                                    FirstName = importedUSOhioCity.CouncilMember12FirstName,
                                    MiddleName = importedUSOhioCity.CouncilMember12MiddleName,
                                    LastName = importedUSOhioCity.CouncilMember12LastName,
                                    MunicipalityId = city.Id,
                                    Title = importedUSOhioCity.CouncilMember12Title
                                };
                                termEndDate = DateTime.MinValue;
                                if (DateTime.TryParse(importedUSOhioCity.CouncilMember12TermEndDate, out termEndDate))
                                {
                                    municipalCouncilMember12.TermEndDate = termEndDate;
                                    municipalCouncilMember12.TermStartDate = termEndDate.AddYears(-4);
                                }
                                context.MunicipalCouncilMembers.Add(municipalCouncilMember12);
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

        public static void LoadUSOhioVillages(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!context.Municipalities.Any(x => x.MunicipalityTypeId == (int)MunicipalityTypeEnum.Village))
                    {
                        var countryRepository = new CountryRepository(context);
                        var country = countryRepository.GetByAbbreviation("US");
                        var stateRepository = new StateProvinceRepository(context);
                        var state = stateRepository.Get(country, "Ohio");
                        var usOhioVillagesData = File.ReadAllText(path + @"/Content/data/json/USA-Ohio-Village-Officials-2022-2023.json");
                        var importedUSOhioVillages = JsonSerializer.Deserialize<List<ImportedVillage>>(usOhioVillagesData);
                        foreach (var importedVillage in importedUSOhioVillages)
                        {
                            var village = new Municipality
                            {
                                Census2000 = int.Parse(importedVillage.Census2000.Replace(",", string.Empty)),
                                Census2010 = int.Parse(importedVillage.Census2010.Replace(",", string.Empty)),
                                Census2020 = int.Parse(importedVillage.Census2020.Replace(",", string.Empty)),
                                FormOfGovernment = importedVillage.FormOfGovernment,
                                MailingAddressLine1 = importedVillage.Address1,
                                MailingAddressLine2 = importedVillage.Address2,
                                MunicipalityTypeId = (int)MunicipalityTypeEnum.Village,
                                Name = importedVillage.VillageName,
                                StateId = state.Id,
                                Telephone = importedVillage.Telephone,
                                Website = importedVillage.Website,
                                YearIncorporated = importedVillage.YearIncorporated,
                                ZIPCode = importedVillage.ZIPCode.GetValueOrDefault(),
                            };
                            context.Municipalities.Add(village);
                            if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static void LoadUSOhioMunicipalTaxRates(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var municipalTaxRatesData = File.ReadAllText(path + @"/Content/data/json/USA-Ohio-MunicipalTaxRates-2023-05-24.json");
                    var importedMunicipalTaxRates = JsonSerializer.Deserialize<List<ImportedMunicipalTaxRate>>(municipalTaxRatesData);
                    if (importedMunicipalTaxRates == null) return;
                    if (importedMunicipalTaxRates.Count == 0) return;
                    var countryRepository = new CountryRepository(context);
                    var country = countryRepository.GetByAbbreviation("US");
                    var stateRepository = new StateProvinceRepository(context);
                    var state = stateRepository.Get(country, "Ohio");
                    var municipalalityRepository = new MunicipalityRepository(context);
                    var unknownMunicipalities = new List<string>();
                    foreach (var importedMunicipalTaxRate in importedMunicipalTaxRates)
                    {
                        var municipality = municipalalityRepository.Get(state, importedMunicipalTaxRate.MunicipalityName);
                        if (municipality == null)
                        {
                            unknownMunicipalities.Add(importedMunicipalTaxRate.MunicipalityName);
                            continue;
                        }
                        var municipalIncomeTaxRate = new MunicipalIncomeTaxRate
                        {
                            MunicipalityId = municipality.Id,
                            Rate = importedMunicipalTaxRate.TaxRate,
                            StartDate = DateTime.ParseExact(importedMunicipalTaxRate.StartDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture)
                        };
                        var endDateAsString = importedMunicipalTaxRate.EndDate.ToString();
                        if (!endDateAsString.StartsWith("9999"))
                        {
                            DateTime endDate = DateTime.MinValue;
                            if (DateTime.TryParseExact(importedMunicipalTaxRate.EndDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                            {
                                municipalIncomeTaxRate.EndDate = endDate;
                            }
                        }
                        context.MunicipalIncomeTaxRates.Add(municipalIncomeTaxRate);
                    }
                    if (context.ChangeTracker.HasChanges()) context.SaveChanges();
                    if (unknownMunicipalities.Count > 0)
                    {

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
