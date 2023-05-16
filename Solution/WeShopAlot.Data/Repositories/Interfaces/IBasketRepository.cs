using WeShopAlot.Data.Models;

namespace WeShopAlot.Data.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);

        Task<bool> DeleteBasketAsync(string basketId);
    }
}
