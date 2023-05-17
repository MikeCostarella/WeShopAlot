using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.Extensions.ModelBuilderFunctionality
{
    public static class ModelBuilderForeignKeyExtensions
    {

        public static void InitializeWeShopAlotObjectForeignKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.State).WithMany(a => a.Counties).HasForeignKey(b => b.StateId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Municipality>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.MunicipalityType).WithMany().HasForeignKey("MunicipalityTypeId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Municipality>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.State).WithMany(a => a.Municipalities).HasForeignKey(b => b.StateId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Municipalities).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Counties).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Precinct>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Precincts).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<State>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.Country).WithMany(a => a.States).HasForeignKey(b => b.CountryId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Township>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Townships).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
