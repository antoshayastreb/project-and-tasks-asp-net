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


}
