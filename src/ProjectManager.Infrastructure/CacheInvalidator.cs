using Microsoft.Extensions.Caching.Memory;
using ProjectManager.Application.Caching;

namespace ProjectManager.Infrastructure;

/// <summary>
/// 
/// </summary>
public class CacheInvalidator : ICacheInvalidator
{
    private readonly IMemoryCache _cache;

    public CacheInvalidator(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void InvalidateCache(object key)
    {
        _cache.Remove(key);
    }
}
