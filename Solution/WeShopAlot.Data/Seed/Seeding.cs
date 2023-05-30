using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Shared.Enumerations;

namespace WeShopAlot.Data.Seed
{
    public static partial class Seeding
    {
        public static void SeedMasterData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderStatus>().HasData(EnumSeedingExtension.SeedEnumValues<OrderStatusEnum, OrderStatus>());
        }
    }
}
