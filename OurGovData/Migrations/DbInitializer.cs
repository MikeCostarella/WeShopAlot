using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MeShopAlot.Data.Migrations
{
    public static partial class DbInitializer
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                mutableEntityType.SetTableName(mutableEntityType.DisplayName());
            }
        }
    }
}
