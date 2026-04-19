using System;
using ProjectManager.Application.DTOs.Project;
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
}
