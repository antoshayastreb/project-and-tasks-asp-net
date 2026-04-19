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
    public Task<IReadOnlyList<ProjectListDto>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default
    )
    {
        return _projectQueries.GetAllAsync(page, pageSize, ct);
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

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateProjectDto dto,
        CancellationToken ct = default
    )
    {
        var id = await _projectService.CreateAsync(dto, ct);
        return id;
    }
}
