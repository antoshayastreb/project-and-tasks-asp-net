using System;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Application.Services;

public class ProjectService : IProjectService
{
    private IUnitOfWork _uow;

    public ProjectService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Guid> CreateAsync(CreateProjectDto dto, CancellationToken ct)
    {
        var project = new Project(name: dto.Name, description: dto.Description);
        await _uow.Projects.AddAsync(project, ct);
        await _uow.SaveChangesAsync(ct);
        return project.Id;
    }

    public async Task RemoveAsync(Guid id, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(id, ct);
        if (project is null)
            throw new NotFoundException(nameof(Project), id);
        _uow.Projects.Remove(project);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Guid id, UpdateProjectDto dto, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(id, ct);
        if (project is null)
            throw new NotFoundException(nameof(Project), id);

        if (!String.IsNullOrEmpty(dto.Name))
            project.ChangeName(dto.Name);
        project.ChangeDescription(dto.Description);
        await _uow.SaveChangesAsync(ct);
    }
}
