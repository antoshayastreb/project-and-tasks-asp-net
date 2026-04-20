using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Queries;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Api.Controllers
{
    /// <summary>
    /// Контролер реализующий API для ProjectTasks.
    /// </summary>
    [Route("api/tasks")]
    [ApiController]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _service;
        private readonly IProjectTaskQueries _queries;

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
        public async Task<ActionResult<Guid>> Create(
            [FromBody] CreateProjectTaskDto dto,
            CancellationToken ct = default
        )
        {
            return await _service.CreateAsync(dto, ct);
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
        public async Task Update(
            Guid id,
            [FromBody] UpdateProjectTaskDto dto,
            CancellationToken ct = default
        )
        {
            await _service.UpdateAsync(id, dto, ct);
        }

        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(
            Guid id,
            CancellationToken ct = default
        )
        {
            await _service.RemoveAsync(id, ct);
        }
    }
}
