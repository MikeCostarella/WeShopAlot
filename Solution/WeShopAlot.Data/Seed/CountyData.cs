using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Shared.Enumerations.OhioSpecific;
using WeShopAlot.Shared.Enumerations;

namespace WeShopAlot.Data.Seed
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
