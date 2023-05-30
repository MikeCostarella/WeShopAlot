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

        #region Factory

        public static WeShopAlotContext NewFromConnectionString(string connectionString)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<WeShopAlotContext>();
            dbContextOptionsBuilder.UseSqlServer(connectionString);
            var dbContext = new WeShopAlotContext(dbContextOptionsBuilder.Options);
            return dbContext;
        }

        #endregion Factory

        #region DBSets

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<CustomerBasket> CustomerBaskets { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductItemOrdered> ProductItemsOrdered { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

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