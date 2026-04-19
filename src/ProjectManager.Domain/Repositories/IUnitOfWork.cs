using System;

namespace ProjectManager.Domain.Repositories;

public interface IUnitOfWork
{
    public IProjectRepository Projects { get; }

    /// <summary>
    /// Сохранение изменений в базу данных.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task SaveChangesAsync(CancellationToken ct);
}
