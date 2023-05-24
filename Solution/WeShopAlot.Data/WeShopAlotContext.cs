using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Migrations;
using WeShopAlot.Data.Seed;
using System.Configuration;
using WeShopAlot.Data.Extensions.ModelBuilderFunctionality;

namespace WeShopAlot.Data
{
    public class WeShopAlotContext : DbContext
    {
        #region Constructors

        public WeShopAlotContext(DbContextOptions<WeShopAlotContext> options) : base(options)
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
        public DbSet<GovernmentScope> GovernmentScopes { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Mayor> Mayors { get; set; }
        public DbSet<MunicipalAuditor> MunicipalAuditors { get; set; }
        public DbSet<MunicipalCouncilMember> MunicipalCouncilMembers { get; set; }
        public DbSet<MunicipalCouncilPresident> MunicipalCouncilPresidents { get; set; }
        public DbSet<MunicipalIncomeTaxRate> MunicipalIncomeTaxRates { get; set; }
        public DbSet<MunicipalLawDirector> MunicipalLawDirectors { get; set; }
        public DbSet<MunicipalTreasurer> MunicipalTreasurers { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<MunicipalityCounty> MunicipalityCounties { get; set; }
        public DbSet<MunicipalityType> MunicipalityTypes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Precinct> Precincts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductItemOrdered> ProductItemsOrdered { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }
        public DbSet<Township> Townships { get; set; }
        public DbSet<TownshipFiscalOfficer> TownshipFiscalOfficers { get; set; }
        public DbSet<TownshipTrustee> TownshipTrustees { get; set; }

        #endregion DbSets

        #region Initialization

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WeShopAlotContext>(options =>
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["WeShopAlotSQLConnection"].ConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("WSA");
            modelBuilder.RemovePluralizingTableNameConvention();
            Seeding.SeedMasterData(modelBuilder);
            modelBuilder.InitializeWeShopAlotEnumerations();
            modelBuilder.InitializeWeShopAlotObjectForeignKeys();
        }

        #endregion Initialization
    }
}