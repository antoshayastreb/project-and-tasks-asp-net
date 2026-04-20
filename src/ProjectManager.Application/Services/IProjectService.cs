using System;
using ProjectManager.Application.DTOs.Project;

namespace ProjectManager.Application.Services;

public interface IProjectService
{

    /// <summary>
    /// Метод создания проекта.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Guid> CreateAsync(CreateProjectDto dto, CancellationToken ct);

    /// <summary>
    /// Метод обновления проекта.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task UpdateAsync(Guid id, UpdateProjectDto dto, CancellationToken ct);

    /// <summary>
    /// Метод удаления проекта.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task RemoveAsync(Guid id, CancellationToken ct);
}
