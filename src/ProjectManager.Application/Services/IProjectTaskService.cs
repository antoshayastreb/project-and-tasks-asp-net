using ProjectManager.Application.DTOs.ProjectTask;

namespace ProjectManager.Application.Services;

public interface IProjectTaskService
{

    /// <summary>
    /// Метод создания задачи.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Guid> CreateAsync(CreateProjectTaskDto dto, CancellationToken ct);

    /// <summary>
    /// Метод обновления задачи.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task UpdateAsync(Guid id, UpdateProjectTaskDto dto, CancellationToken ct);

    /// <summary>
    /// Метод удаления задачи.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task RemoveAsync(Guid id, CancellationToken ct);
}
