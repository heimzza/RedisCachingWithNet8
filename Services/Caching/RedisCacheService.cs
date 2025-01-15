using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisCachingWithNet8.Services.Caching;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public T? GetData<T>(string key)
    {
        var data = _cache?.GetString(key);

        return data is null 
            ? default 
            : JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        
        _cache?.SetString(key, JsonSerializer.Serialize(data), options);
    }
}