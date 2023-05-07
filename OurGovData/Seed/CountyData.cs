using Microsoft.EntityFrameworkCore;
using MeShopAlot.Data.Models;
using MeShopAlot.Shared.Enumerations;
using MeShopAlot.Shared.Enumerations.Counties;

namespace MeShopAlot.Data.Seed
{
    public static partial class Seeding
    {
        internal static void CountyData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>().HasData(
                new County { Id = (int)OhioCountyEnum.Adams, Name = OhioCountyEnum.Adams, StateId = (int) StateEnum.Ohio },
                new County { Id = (int)OhioCountyEnum.Allen, Name = OhioCountyEnum.Allen, StateId = (int)StateEnum.Ohio }
            );
        }
    }
}
