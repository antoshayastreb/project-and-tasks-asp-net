using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.Queries;
using ProjectManager.Application.Services;

namespace ProjectManager.Api.Controllers;

/// <summary>
/// Контролер реализующий API для Projects.
/// </summary>
[Route("api/projects")]
[ApiController]
public class ProjectsController : ControllerBase
{

    private readonly IProjectQueries _projectQueries;

    private readonly IProjectService _projectService;

    public ProjectsController(IProjectQueries queries, IProjectService service)
    {
        _projectQueries = queries;
        _projectService = service;
    }

    /// <summary>
    /// Получить список проектов с пагинацией.
    /// </summary>
    /// <param name="page">Номер страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IReadOnlyList<ProjectListDto>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default
    )
    {
        return await _projectQueries.GetAllAsync(page, pageSize, ct);
    }

    /// <summary>
    /// Получить проект по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ProjectDto?>> Get(
        Guid id,
        CancellationToken ct = default
    )
    {
        var project = await _projectQueries.GetByIdAsync(id, ct);
        return project is null ? NotFound() : Ok(project);
    }

    /// <summary>
    /// Создать проект.
    /// </summary>
    /// <param name="dto">Тело запроса (dto)</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateProjectDto dto,
        CancellationToken ct = default
    )
    {
        var id = await _projectService.CreateAsync(dto, ct);
        return id;
    }

    /// <summary>
    /// Обновить проект.
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <param name="dto">Тело запроса (dto)</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(
        Guid id, 
        [FromBody] UpdateProjectDto dto,
        CancellationToken ct = default
    )
    {
        await _projectService.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    /// <summary>
    /// Удалить проект и все его задачи.
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct = default
    )
    {
        await _projectService.RemoveAsync(id, ct);
        return NoContent();
    }
}
