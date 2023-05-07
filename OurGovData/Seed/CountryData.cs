using Microsoft.EntityFrameworkCore;
using MeShopAlot.Shared.Enumerations;
using MeShopAlot.Data.Models;

namespace MeShopAlot.Data.Seed
{
    public static partial class Seeding
    {
        internal static void CountryData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = (int)CountryEnum.Canada, Abbreviation = "CAN", Name = CountryEnum.Canada },
                new Country { Id = (int)CountryEnum.UnitedStates, Abbreviation = "USA", Name = CountryEnum.UnitedStates }
            );
        }
    }
}
