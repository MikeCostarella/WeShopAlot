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
                    if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
                    {
                        context.Database.ExecuteSqlRaw(File.ReadAllText(dir + @"\Scripts\DropSQLServerDatabase.sql"));
                    }
                    if (context.Database.ProviderName != "Npgsql.EntityFrameworkCore.PostgreSQL")
                    {
                        // ToDo: write a script to delete all tables from postgres db
                    }
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
