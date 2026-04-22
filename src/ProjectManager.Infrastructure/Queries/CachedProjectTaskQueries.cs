using Microsoft.Extensions.Caching.Memory;
using ProjectManager.Application.Caching;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Queries;

namespace ProjectManager.Infrastructure.Queries;

/// <summary>
/// Кэшированные запросы получения задач из хранилища.
/// </summary>
public class CachedProjectTaskQueries : IProjectTaskQueries
{

    private readonly IProjectTaskQueries _innerQueries;
    private readonly IMemoryCache _cache;

    public CachedProjectTaskQueries(IMemoryCache cache, IProjectTaskQueries innerQueries)
    {
        _innerQueries = innerQueries;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ProjectTaskDto>> GetAllAsync(Guid? projectId, bool? isCompleted, CancellationToken ct)
    {
        //Поскольку сложно наверняка инвалидировать кэш списка с фильтрацией
        //кэш для этого запроса отключен
        return await _innerQueries.GetAllAsync(projectId: projectId, isCompleted: isCompleted, ct);
    }

    public async Task<ProjectTaskDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _cache.GetOrCreateAsync(CacheKeys.ProjectTaskById(id), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return _innerQueries.GetByIdAsync(id, ct);
        });
    }
}
