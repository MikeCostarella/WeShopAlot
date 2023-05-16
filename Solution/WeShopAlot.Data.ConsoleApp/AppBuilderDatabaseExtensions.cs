using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WeShopAlot.Data.ConsoleApp
{
    public static class AppBuilderDatabaseExtensions
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                context.Database.ExecuteSqlRaw(File.ReadAllText(dir + @"\Scripts\DropDatabase.sql"));
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                context.Database.Migrate();
            }
        }
    }
}
