
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория сущности Project.
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Получение сущности Project по идентификатору.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Добавляет сущность Project в хранилище.
    /// </summary>
    /// <param name="project"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddAsync(Project project, CancellationToken ct);

    /// <summary>
    /// Удаляет сущность Project из хранилища.
    /// </summary>
    /// <param name="project"></param>
    void Remove(Project project);
    
    /// <summary>
    /// Проверка на существование Project.
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}
