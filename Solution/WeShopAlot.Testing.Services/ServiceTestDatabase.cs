using WeShopAlot.Data.Seed;

namespace WeShopAlot.Testing.Services
{
    /// <summary>
    /// A WeShopAlot database on the EF Core in-memory provider, loaded with the shipped catalog by
    /// the same <see cref="DatabaseSeeder"/> the console app and the SQL Server test tier use:
    /// 16 products, 10 brands, 7 types, 4 delivery methods, and the 3 order statuses that
    /// <c>HasData</c> puts in the model.
    ///
    /// Each instance gets its own database name, so tests never see each other's rows, and
    /// <see cref="CreateContext"/> can be called as often as a test likes: every context on the
    /// same name reads the same data, which is how a test checks what a service really saved
    /// instead of what its own change tracker still holds.
    ///
    /// What this is not: a database. The in-memory provider runs no migrations and enforces no
    /// foreign keys, column sizes, or SQL ordering. A service test here proves the service's
    /// logic (which rows it reads, what it computes, what it hands to SaveChanges). Whether SQL
    /// Server accepts those rows is the SQL Server tier's question (<c>SqlApiTests</c>).
    /// </summary>
    internal sealed class ServiceTestDatabase
    {
        private readonly DbContextOptions<WeShopAlotContext> options;

        private ServiceTestDatabase(DbContextOptions<WeShopAlotContext> options)
        {
            this.options = options;
        }

        /// <summary>Creates an empty in-memory database, applies the model's <c>HasData</c> rows, and seeds the catalog.</summary>
        public static async Task<ServiceTestDatabase> CreateSeededAsync()
        {
            var options = new DbContextOptionsBuilder<WeShopAlotContext>()
                .UseInMemoryDatabase("ServiceTests-" + Guid.NewGuid())
                .Options;

            await using (var db = new WeShopAlotContext(options))
            {
                // EnsureCreated, not Migrate: the in-memory provider has no migrations to run.
                // It does apply HasData, which is where the OrderStatus rows come from.
                await db.Database.EnsureCreatedAsync();
                await DatabaseSeeder.SeedAsync(db);
            }

            return new ServiceTestDatabase(options);
        }

        /// <summary>A new context on this test's database. Dispose it when done.</summary>
        public WeShopAlotContext CreateContext() => new WeShopAlotContext(options);
    }
}
