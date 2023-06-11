using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;

namespace WeShopAlot.Data.Repositories.Interfaces;

public interface IAppUserRepository : IGenericRepository<AppUser>
{
    bool EmailExists(string emailAddress);

    Task<bool> EmailExistsAsync(string emailAddress);

    Task<AppUser> GetByEmailAddressAsync(string emailAddress);

    Task<AppUser> InsertAsync(AppUser appUser);
}
