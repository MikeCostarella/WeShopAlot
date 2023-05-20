using Microsoft.EntityFrameworkCore;
using WeShopAlot.Shared.Enumerations;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Shared.Enumerations;

namespace WeShopAlot.Data.Seed
{
    public static partial class Seeding
    {
        public static void SeedMasterData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GovernmentScope>().HasData(EnumSeedingExtension.SeedEnumValues<GovernmentScopeEnum, GovernmentScope>());
            modelBuilder.Entity<MunicipalityType>().HasData(EnumSeedingExtension.SeedEnumValues<MunicipalityTypeEnum, MunicipalityType>());
            modelBuilder.Entity<OrderStatus>().HasData(EnumSeedingExtension.SeedEnumValues<OrderStatusEnum, OrderStatus>());
        }
    }
}
