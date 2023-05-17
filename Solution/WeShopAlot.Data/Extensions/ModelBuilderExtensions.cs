using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Seed.Common;
using WeShopAlot.Data.Shared.Enumerations;
using WeShopAlot.Shared.Enumerations;

namespace WeShopAlot.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void InitializeWeShopAlotEnumerations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().Property(e => e.Name).HasConversion(EnumX.Converter<CountryEnum>());
            modelBuilder.Entity<MunicipalityType>().Property(e => e.Name).HasConversion(EnumX.Converter<MunicipalityTypeEnum>());
            modelBuilder.Entity<OrderStatus>().Property(e => e.Name).HasConversion(EnumX.Converter<OrderStatusEnum>());
            modelBuilder.Entity<State>().Property(e => e.Name).HasConversion(EnumX.Converter<StateEnum>());
        }

        public static void InitializeWeShopAlotObjectForeignKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.State).WithMany(a => a.Counties).HasForeignKey(b => b.StateId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Municipality>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.MunicipalityType).WithMany().HasForeignKey("MunicipalityTypeId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Municipality>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.State).WithMany(a => a.Municipalities).HasForeignKey(b => b.StateId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Municipalities).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Counties).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<State>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.Country).WithMany(a => a.States).HasForeignKey(b => b.CountryId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Township>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Townships).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
