using ProjectManager.Application.DTOs.ProjectTask;

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

    /// <summary>
    /// Возвращает список задач с фильтрами.
    /// </summary>
    /// <param name="projectId">Фильтрация по проекту</param>
    /// <param name="isCompleted">Фильтрация по статусу</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ProjectTaskDto>> GetAllAsync(Guid? projectId, bool? isCompleted, CancellationToken ct);
}
