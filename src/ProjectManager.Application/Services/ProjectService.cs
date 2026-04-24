using FluentValidation;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Caching;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Application.Services;

public class ProjectService : IProjectService
{
    private readonly ILogger<ProjectService> _logger;
    private readonly ICacheInvalidator _cacheInvalidator;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectService(
        IUnitOfWork uow,
        ILogger<ProjectService> logger,
        IValidator<CreateProjectDto> createValidator,
        IValidator<UpdateProjectDto> updateValidator,
        ICacheInvalidator cacheInvalidator
    )
    {
        _uow = uow;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _cacheInvalidator = cacheInvalidator;
    }

    public async Task<Guid> CreateAsync(CreateProjectDto dto, CancellationToken ct)
    {
        _logger.LogInformation("Creating Project");

        await _createValidator.ValidateAndThrowAsync(dto, ct);

        var project = new Project(name: dto.Name, description: dto.Description);
        await _uow.Projects.AddAsync(project, ct);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Project {ProjectId} created", 
            project.Id
        );
        
        return project.Id;
    }

    public async Task RemoveAsync(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Deleting Project {ProjectId}", 
            id
        );

        var project = await _uow.Projects.GetByIdAsync(id, ct);
        if (project is null)
            throw new NotFoundException(nameof(Project), id);
        _uow.Projects.Remove(project);

        _logger.LogInformation("Project {ProjectId} deleted", id);

        await _uow.SaveChangesAsync(ct);

        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectById(id));
    }

    public async Task UpdateAsync(Guid id, UpdateProjectDto dto, CancellationToken ct)
    {
        _logger.LogInformation("Updating Project {ProjectId}", id);

        await _updateValidator.ValidateAndThrowAsync(dto, ct);

        var project = await _uow.Projects.GetByIdAsync(id, ct);
        if (project is null)
            throw new NotFoundException(nameof(Project), id);

        if (!String.IsNullOrEmpty(dto.Name))
            project.ChangeName(dto.Name);
        project.ChangeDescription(dto.Description);

        _logger.LogInformation("Project {ProjectId} updated", 
            project.Id
        );

        await _uow.SaveChangesAsync(ct);

        _cacheInvalidator.InvalidateCache(CacheKeys.ProjectById(id));
    }
}
