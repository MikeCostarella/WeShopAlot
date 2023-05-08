using Microsoft.EntityFrameworkCore;
using MeShopAlot.Data.Models;
using MeShopAlot.Shared.Enumerations;

namespace MeShopAlot.Data.Seed
{
    public static partial class Seeding
    {
        internal static void StateData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>().HasData(
                new State { Id = (int)StateEnum.Alabama, Abbreviation = "AL", Name = StateEnum.Alabama, CountryId = (int) CountryEnum.UnitedStates },
                new State { Id = (int)StateEnum.Alaska, Abbreviation = "AK", Name = StateEnum.Alaska, CountryId = (int)CountryEnum.UnitedStates },
                new State { Id = (int)StateEnum.Ohio, Abbreviation = "OH", Name = StateEnum.Ohio, CountryId = (int)CountryEnum.UnitedStates }
            );
        }
    }
}
