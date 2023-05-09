using MeShopAlot.Data.Models;

namespace MeShopAlot.Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
