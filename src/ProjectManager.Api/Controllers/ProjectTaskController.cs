using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Models;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Queries;
using ProjectManager.Application.Services;

namespace ProjectManager.Api.Controllers
{
    /// <summary>
    /// Контролер реализующий API для ProjectTasks.
    /// </summary>
    [Route("api/tasks")]
    [ApiController]
    [Produces("application/json")]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _service;
        private readonly IProjectTaskQueries _queries;
        
        /// <inheritdoc/>
        public ProjectTaskController(IProjectTaskService service, IProjectTaskQueries queries)
        {
            _service = service;
            _queries = queries;
        }

        /// <summary>
        /// Получить задачу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProjectTaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectTaskDto?>> Get(
            Guid id,
            CancellationToken ct = default
        )
        {
            var task = await _queries.GetByIdAsync(id, ct);
            return task is null ? NotFound() : Ok(task);
        }

        /// <summary>
        /// Получить список задач с фильтрацией.
        /// </summary>
        /// <param name="projectId">Фильтр по проекту</param>
        /// <param name="IsCompleted">Фильтр по состоянию</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProjectTaskDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IReadOnlyList<ProjectTaskDto>> GetAll(
            [FromQuery] Guid? projectId,
            [FromQuery] bool? IsCompleted,
            CancellationToken ct = default
        )
        {
            return await _queries.GetAllAsync(projectId: projectId, isCompleted: IsCompleted, ct);
        }

        /// <summary>
        /// Создать задачу.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Create(
            [FromBody] CreateProjectTaskDto dto,
            CancellationToken ct = default
        )
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        /// <summary>
        /// Обновить задачу.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(
            Guid id,
            [FromBody] UpdateProjectTaskDto dto,
            CancellationToken ct = default
        )
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]        
        public async Task<ActionResult> Delete(
            Guid id,
            CancellationToken ct = default
        )
        {
            await _service.RemoveAsync(id, ct);
            return NoContent();
        }
    }
}
