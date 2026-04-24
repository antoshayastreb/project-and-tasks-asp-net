using System;

namespace ProjectManager.Domain.Repositories;

public interface IUnitOfWork
{
    /// <summary>
    /// Репозиторий для работы с проектами.
    /// </summary>
    public IProjectRepository Projects { get; }

    /// <summary>
    /// Репозиторий для работы с задачами.
    /// </summary>
    public IProjectTaskRepository ProjectTasks { get; }

    /// <summary>
    /// Сохранение изменений в базу данных.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task SaveChangesAsync(CancellationToken ct);
}
