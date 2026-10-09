using Microsoft.EntityFrameworkCore;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Base;
using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Data.Repositories;

public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
{
    #region Constructors

    public AppUserRepository(WeShopAlotContext context) : base(context) { }

    #endregion Constructors

    #region Public Methods

    public bool EmailExists(string emailAddress)
    {
        return dbContext.AppUsers.FirstOrDefault(x => x.EmailAddress.Equals(emailAddress)) != null;
    }

    public async Task<bool> EmailExistsAsync(string emailAddress)
    {
        return (await dbContext.AppUsers.FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress))) != null;
    }

    public Task<AppUser> GetByEmailAddressAsync(string emailAddress)
    {
        return dbContext.AppUsers.Include(x => x.Address).FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress));
    }

    public async Task<AppUser> InsertAsync(AppUser appUser)
    {
        dbContext.AppUsers.Add(appUser);
        await dbContext.SaveChangesAsync();
        return appUser;
    }

    #endregion Public Methods
}
