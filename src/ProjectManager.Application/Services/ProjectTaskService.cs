using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Application.Services;

public class ProjectTaskService : IProjectTaskService
{

    private readonly IUnitOfWork _uow;

    public ProjectTaskService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Guid> CreateAsync(CreateProjectTaskDto dto, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(dto.ProjectId, ct);
        if (project is null)
            throw new NotFoundException(nameof(Project), dto.ProjectId);
        var task = project.AddTask(
            title: dto.Title,
            description: dto.Description,
            isCompleted: dto.IsCompleted
        );
        await _uow.SaveChangesAsync(ct);
        return task.Id;
    }

    public async Task RemoveAsync(Guid id, CancellationToken ct)
    {
        var task = await _uow.ProjectTasks.GetByIdAsync(id, ct);
        if (task is null)
            throw new NotFoundException(nameof(ProjectTask), id);
        _uow.ProjectTasks.Remove(task);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Guid id, UpdateProjectTaskDto dto, CancellationToken ct)
    {
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
        await _uow.SaveChangesAsync(ct);
    }
}
