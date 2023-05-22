using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WeShopAlot.Data.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static void UseSelectedDatabaseServer(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration config)
        {
            var selectedDbServerType = config["Settings:DbServerType"];
            switch (selectedDbServerType)
            {
                case "SQLServer":
                    dbContextOptionsBuilder.UseSqlServer(config.GetConnectionString("WeShopAlotSQLConnection"));
                    break;
                case "PostgresSql":
                    dbContextOptionsBuilder.UseNpgsql(config.GetConnectionString("WeShopAlotNPGConnection"));
                    break;
                default:
                    break;
            }
        }
    }
}
