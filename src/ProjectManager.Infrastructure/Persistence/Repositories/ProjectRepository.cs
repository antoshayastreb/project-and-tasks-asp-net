using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Infrastructure.Persistence.Repositories;

/// <summary>
/// Реализация репозитория для сущности Project
/// </summary>
public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _dbContext;
    
    public ProjectRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Project project, CancellationToken ct)
    {
        await _dbContext.Projects.AddAsync(project, ct);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Projects.Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public void Remove(Project project)
    {
        _dbContext.Projects.Remove(project);
    }
}
