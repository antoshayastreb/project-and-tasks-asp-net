using System;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Infrastructure.Persistence.Repositories;

/// <summary>
/// Реализация репозитория для сущности ProjectTask
/// </summary>
public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly AppDbContext _dbContext;

    public ProjectTaskRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ProjectTask projectTask, CancellationToken ct)
    {
        await _dbContext.ProjectTasks.AddAsync(projectTask, ct);
    }

    public async Task<ProjectTask?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.ProjectTasks.FindAsync([id], ct);
    }

    public void Remove(ProjectTask projectTask)
    {
        _dbContext.ProjectTasks.Remove(projectTask);
    }
}
