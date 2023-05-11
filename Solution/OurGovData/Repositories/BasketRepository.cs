using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace MeShopAlot.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        // ToDo: change id back to int
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id.ToString(), JsonSerializer.Serialize(basket),
                TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetBasketAsync(basket.Id.ToString());
        }
    }
}
