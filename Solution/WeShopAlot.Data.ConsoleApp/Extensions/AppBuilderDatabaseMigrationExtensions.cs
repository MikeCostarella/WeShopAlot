using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WeShopAlot.Data.ConsoleApp.Extensions
{
    public static class AppBuilderDatabaseMigrationExtensions
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var context = servicedScope.ServiceProvider.GetRequiredService<WeShopAlotContext>();
                try
                {
                    context.Database.ExecuteSqlRaw(File.ReadAllText(dir + @"\Scripts\DropDatabase.sql"));
                }
                catch (Exception ex)
                {
                    throw;
                }
                context.Database.Migrate();
            }
        }
    }
}
