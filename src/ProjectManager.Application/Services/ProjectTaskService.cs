using FluentValidation;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Caching;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Application.Services;

public class ProjectTaskService : IProjectTaskService
{
    private ILogger<ProjectTaskService> _logger;
    private readonly IUnitOfWork _uow;
    private readonly ICacheInvalidator _cacheInvalidator;

    private readonly IValidator<CreateProjectTaskDto> _createValidator;
    private readonly IValidator<UpdateProjectTaskDto> _updateValidator;    

    public ProjectTaskService(
        IUnitOfWork uow,
        ILogger<ProjectTaskService> logger,
        IValidator<CreateProjectTaskDto> createValidator,
        IValidator<UpdateProjectTaskDto> updateValidator,
        ICacheInvalidator cacheInvalidator
    )
    {
        _uow = uow;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _cacheInvalidator = cacheInvalidator;
    }

    public async Task<Guid> CreateAsync(CreateProjectTaskDto dto, CancellationToken ct)
    {
        _logger.LogInformation("Creating ProjectTask for Project {ProjectId}", 
            dto.ProjectId
        );

        await _createValidator.ValidateAndThrowAsync(dto, ct);

        var project = await _uow.Projects.GetByIdAsync(dto.ProjectId, ct);
        if (project is null)
            throw new NotFoundException(nameof(Project), dto.ProjectId);
        var task = project.AddTask(
            title: dto.Title,
            description: dto.Description,
            isCompleted: dto.IsCompleted
        );

        _logger.LogInformation("ProjectTask {ProjectTaskId} for Project {ProjectId} created", 
            task.Id, 
            project.Id
        );

        await _uow.SaveChangesAsync(ct);

        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectById(project.Id));

        return task.Id;
    }

    public async Task RemoveAsync(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Deleting ProjectTask {ProjectTaskId}", 
            id
        );

        var task = await _uow.ProjectTasks.GetByIdAsync(id, ct);
        if (task is null)
            throw new NotFoundException(nameof(ProjectTask), id);
        _uow.ProjectTasks.Remove(task);

        _logger.LogInformation("ProjectTask {ProjectTaskId} deleted", 
            task.Id
        );

        await _uow.SaveChangesAsync(ct);

        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectById(task.ProjectId));
        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectTaskById(task.Id));
    }

    public async Task UpdateAsync(Guid id, UpdateProjectTaskDto dto, CancellationToken ct)
    {
        _logger.LogInformation("Updating ProjectTask {ProjectTaskId} for Project {ProjectId}", 
            id, 
            dto.ProjectId
        );

        await _updateValidator.ValidateAndThrowAsync(dto, ct);

        var task = await _uow.ProjectTasks.GetByIdAsync(id, ct);
        if (task is null)
            throw new NotFoundException(nameof(ProjectTask), id);
        if (!String.IsNullOrEmpty(dto.Title))
            task.ChangeTitle(dto.Title);
        task.ChangeDescription(dto.Description);
        if (dto.IsCompleted.HasValue)
            task.ChangeIsCompleted(dto.IsCompleted.Value);
        if (dto.ProjectId.HasValue)
        {
            var projectExists = await _uow.Projects.ExistsAsync(dto.ProjectId.Value, ct);
            if (projectExists)
                task.ChangeProjectId(dto.ProjectId.Value);
            else
                throw new NotFoundException(nameof(Project), dto.ProjectId.Value);           
        }

        _logger.LogInformation("ProjectTask {ProjectTaskId} for Project {ProjectId} updated", 
            task.Id, 
            dto.ProjectId
        );        

        await _uow.SaveChangesAsync(ct);

        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectById(task.ProjectId));
        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectTaskById(task.Id));
    }
}
