using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MeShopAlot.Data.ConsoleApp
{
    public static class AppBuilderDatabaseExtensions
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var servicedScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var context = servicedScope.ServiceProvider.GetRequiredService<MeShopAlotContext>();
                context.Database.ExecuteSqlRaw(File.ReadAllText(dir + @"\Scripts\DropDatabase.sql"));
                context.Database.Migrate();
            }
        }
    }
}
