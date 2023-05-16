using Microsoft.EntityFrameworkCore;
using WeShopAlot.Shared.Enumerations;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.Seed
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
