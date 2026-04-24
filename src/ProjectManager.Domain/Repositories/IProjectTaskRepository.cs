using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория сущности ProjectTask.
/// </summary>
public interface IProjectTaskRepository
{
    /// <summary>
    /// Получение сущности ProjectTask по идентификатору.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ProjectTask?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Удаляет сущность ProjectTask из хранилища.
    /// </summary>
    /// <param name="projectTask"></param>
    void Remove(ProjectTask projectTask);
}
