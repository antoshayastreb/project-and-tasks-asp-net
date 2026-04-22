using Microsoft.Extensions.Caching.Memory;
using ProjectManager.Application.Caching;
using ProjectManager.Application.DTOs.Project;

namespace ProjectManager.Application.Queries;

/// <summary>
/// Кэшированные запросы получения проектов из хранилища.
/// </summary>
public class CachedProjectQueries : IProjectQueries
{

    private readonly IProjectQueries _innerQueries;
    private readonly IMemoryCache _cache;

    public CachedProjectQueries(IMemoryCache cache, IProjectQueries innerQueries)
    {
        _cache = cache;
        _innerQueries = innerQueries;
    }

    public async Task<IReadOnlyList<ProjectListDto>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        //Поскольку сложно наверняка инвалидировать кэш пагинируемого списка
        //кэш для этого запроса отключен
        return await _innerQueries.GetAllAsync(page: page, pageSize: pageSize, ct: ct);
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _cache.GetOrCreateAsync(CacheKeys.ProjectById(id), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return _innerQueries.GetByIdAsync(id, ct);
        });
    }
}
