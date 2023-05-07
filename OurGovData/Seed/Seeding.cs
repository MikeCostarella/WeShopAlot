using Microsoft.EntityFrameworkCore;
using MeShopAlot.Shared.Enumerations;
using MeShopAlot.Data.Models;

namespace MeShopAlot.Data.Seed
{
    public static partial class Seeding
    {
        public static void SeedMasterData(this ModelBuilder modelBuilder)
        {
            CountryData(modelBuilder);
            StateData(modelBuilder);
            CountyData(modelBuilder);
            modelBuilder.Entity<MunicipalityType>().HasData(EnumSeedingExtension.SeedEnumValues<MunicipalityTypeEnum, MunicipalityType>());
        }
    }
}
