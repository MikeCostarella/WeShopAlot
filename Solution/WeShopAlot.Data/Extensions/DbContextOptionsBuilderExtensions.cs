using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WeShopAlot.Data.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static void UseSelectedDatabaseServer(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration config)
        {
            var selectedDbServerType = config["Settings:DbServerType"];
            if (string.IsNullOrEmpty(selectedDbServerType))
            {
                throw new InvalidOperationException("No database server type was chosen. See secrets file or key vault.");
            }
            string connectionString = null;
            switch (selectedDbServerType)
            {
                case "SQLServer":
                    connectionString = config.GetConnectionString("WeShopAlotSQLConnection");
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException("No SQL server, WeShopAlotSQLConnection, connection string was supplied. See secrets file or key vault.");
                    }
                    dbContextOptionsBuilder.UseSqlServer(connectionString);
                    break;
                case "PostgresSql":
                    connectionString = config.GetConnectionString("WeShopAlotSQLConnection");
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException("No Postgresql server, WeShopAlotNPGConnection, connection string was supplied. See secrets file or key vault.");
                    }
                    dbContextOptionsBuilder.UseNpgsql(connectionString);
                    break;
                default:
                    break;
            }
        }
    }
}
