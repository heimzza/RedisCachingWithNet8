namespace RedisCachingWithNet8.Services.Caching;

public interface IRedisCacheService
{
    T? GetData<T>(string key);
    void SetData<T>(string key, T data);
}