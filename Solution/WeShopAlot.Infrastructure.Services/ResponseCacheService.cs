using WeShopAlot.Data.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace WeShopAlot.Infrastructure.Services;

public class ResponseCacheService : IResponseCacheService
{
    #region Member Variables

    private readonly IDatabase _database;

    #endregion Member Variables

    #region Constructors

    public ResponseCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    #endregion Consructors

    #region Public Methods

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response == null) return;
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var serialisedResponse = JsonSerializer.Serialize(response, options);
        await _database.StringSetAsync(cacheKey, serialisedResponse, timeToLive);
    }

    public async Task<string> GetCachedResponse(string cacheKey)
    {
        var cachedResponse = await _database.StringGetAsync(cacheKey);
        if (cachedResponse.IsNullOrEmpty) return null;
        return cachedResponse;
    }

    #endregion Public Methods
}
