using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Seed.Common;
using WeShopAlot.Data.Shared.Enumerations;
using WeShopAlot.Shared.Enumerations;

namespace WeShopAlot.Data.Extensions.ModelBuilderFunctionality
{
    public static class ModelBuilderEnumerationExtensions
    {
        public static void InitializeWeShopAlotEnumerations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GovernmentScope>().Property(e => e.Name).HasConversion(EnumX.Converter<GovernmentScopeEnum>());
            modelBuilder.Entity<MunicipalityType>().Property(e => e.Name).HasConversion(EnumX.Converter<MunicipalityTypeEnum>());
            modelBuilder.Entity<OrderStatus>().Property(e => e.Name).HasConversion(EnumX.Converter<OrderStatusEnum>());
        }
    }
}
