using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MeShopAlot.Data.Models;
using MeShopAlot.Data.Migrations;
using MeShopAlot.Data.Seed;
using MeShopAlot.Data.Seed.Common;
using MeShopAlot.Shared.Enumerations;
using MeShopAlot.Data.Shared.Enumerations;

namespace MeShopAlot.Data
{
    public class MeShopAlotContext : DbContext
    {
        #region Constructors

        public MeShopAlotContext(DbContextOptions<MeShopAlotContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region DBSets

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<CustomerBasket> CustomerBaskets { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<MunicipalityCounty> MunicipalityCounties { get; set; }
        public DbSet<MunicipalityType> MunicipalityTypes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductItemOrdered> ProductItemsOrdered { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Township> Townships { get; set; }

        #endregion DbSets

        #region Initialization

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<MeShopAlotContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("MeShopAlotDb")));
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MeShopAlotDb"].ConnectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("MSA");
            modelBuilder.RemovePluralizingTableNameConvention();
            Seeding.SeedMasterData(modelBuilder);
            #region Enum Initialization

            modelBuilder.Entity<Country>().Property(e => e.Name).HasConversion(EnumX.Converter<CountryEnum>());
            modelBuilder.Entity<MunicipalityType>().Property(e => e.Name).HasConversion(EnumX.Converter<MunicipalityTypeEnum>());
            modelBuilder.Entity<OrderStatus>().Property(e => e.Name).HasConversion(EnumX.Converter<OrderStatusEnum>());
            modelBuilder.Entity<State>().Property(e => e.Name).HasConversion(EnumX.Converter<StateEnum>());

            #endregion Enum Initialization
            #region Object Relationships

            modelBuilder.Entity<Municipality>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.MunicipalityType).WithMany().HasForeignKey("MunicipalityTypeId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Municipality>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.State).WithMany(a => a.Municipalities).HasForeignKey(b => b.StateId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Municipalities).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MunicipalityCounty>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.Municipality).WithMany(a => a.Counties).HasForeignKey(b => b.MunicipalityId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<State>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.Country).WithMany(a => a.States).HasForeignKey(b => b.CountryId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Township>().HasQueryFilter(b => b.IsDeleted).HasOne(b => b.County).WithMany(a => a.Townships).HasForeignKey(b => b.CountyId).OnDelete(DeleteBehavior.NoAction);

            #endregion Object Relationships
        }

        #endregion Initialization
    }
}