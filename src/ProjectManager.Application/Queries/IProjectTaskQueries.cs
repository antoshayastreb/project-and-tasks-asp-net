using ProjectManager.Application.DTOs;

namespace ProjectManager.Application.Queries;

public interface IProjectTaskQueries
{
    /// <summary>
    /// Возвращает задачу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ProjectTaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
}
