using WeShopAlot.Data.Repositories.Interfaces;

namespace WeShopAlot.Testing.Services
{
    /// <summary>
    /// The basket lives in Redis in production. Here it is a dictionary for the life of the test,
    /// so OrderService can be exercised without a Redis server. The same stand-in exists in
    /// WeShopAlot.Testing.WebApis; it is six lines, so each test project keeps its own.
    /// </summary>
    internal sealed class InMemoryBasketRepository : IBasketRepository
    {
        private readonly Dictionary<string, CustomerBasket> baskets = new();

        public Task<CustomerBasket> GetBasketAsync(string basketId) => Task.FromResult(baskets.GetValueOrDefault(basketId));

        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            baskets[basket.Id] = basket;
            return Task.FromResult(basket);
        }

        public Task<bool> DeleteBasketAsync(string basketId) => Task.FromResult(baskets.Remove(basketId));
    }
}
