using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.Extensions.ModelBuilderFunctionality
{
    public static class ModelBuilderForeignKeyExtensions
    {

        public static void InitializeWeShopAlotObjectForeignKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.StateProvince).WithMany(a => a.Counties).HasForeignKey(b => b.StateProvinceId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Mayor>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Mayors).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalAuditor>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Auditors).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalCouncilMember>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.CouncilMembers).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalCouncilPresident>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.CouncilPresidents).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalLawDirector>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.LawDirectors).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalTreasurer>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Treasurers).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Municipality>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.MunicipalityType).WithMany().HasForeignKey("MunicipalityTypeId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Municipality>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.State).WithMany(a => a.Municipalities).HasForeignKey(b => b.StateId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Municipalities).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Counties).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Precinct>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Precincts).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<StateProvince>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Country).WithMany(a => a.StateProvinces).HasForeignKey(b => b.CountryId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Township>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Townships).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TownshipFiscalOfficer>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Township).WithMany(a => a.FiscalOfficers).HasForeignKey(b => b.TownshipId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TownshipTrustee>().HasQueryFilter(b => !b.IsDeleted).HasOne(b => b.Township).WithMany(a => a.Trustees).HasForeignKey(b => b.TownshipId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
