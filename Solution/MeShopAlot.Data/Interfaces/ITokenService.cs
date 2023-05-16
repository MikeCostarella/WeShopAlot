using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
