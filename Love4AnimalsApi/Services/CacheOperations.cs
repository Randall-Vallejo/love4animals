using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Love4AnimalsApi.Services;

public static class CacheOperations
{
    public static T? TryGet<T>(
        IDistributedCache cache,
        string key,
        JsonSerializerOptions jsonOptions,
        ILogger logger)
    {
        try
        {
            var cached = cache.GetString(key);
            return string.IsNullOrEmpty(cached)
                ? default
                : JsonSerializer.Deserialize<T>(cached, jsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo leer la clave {CacheKey} desde Redis", key);
            return default;
        }
    }

    public static void TrySet<T>(
        IDistributedCache cache,
        string key,
        T value,
        JsonSerializerOptions jsonOptions,
        ILogger logger)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DefaultTtlMinutes)
            };

            cache.SetString(key, JsonSerializer.Serialize(value, jsonOptions), options);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo guardar la clave {CacheKey} en Redis", key);
        }
    }

    public static void TryRemove(IDistributedCache cache, string key, ILogger logger)
    {
        try
        {
            cache.Remove(key);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo invalidar la clave {CacheKey} en Redis", key);
        }
    }
}
