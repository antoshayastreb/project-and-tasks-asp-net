using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Queries;
using ProjectManager.Infrastructure.Persistence;

namespace ProjectManager.Infrastructure.Queries;

public class ProjectTaskQueries : IProjectTaskQueries
{
    private readonly AppDbContext _dbContext;

    public ProjectTaskQueries(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProjectTaskDto>> GetAllAsync(Guid? projectId, bool? isCompleted, CancellationToken ct)
    {
        var query = _dbContext.ProjectTasks.AsNoTracking();
        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);
        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        return await query
            .Select(t => new ProjectTaskDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.ProjectId,
                t.CreatedAt,
                t.UpdatedAt
            )).ToListAsync(ct);
    }

    public async Task<ProjectTaskDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.ProjectTasks.AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new ProjectTaskDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.ProjectId,
                t.CreatedAt,
                t.UpdatedAt
            ))
            .FirstOrDefaultAsync(ct);
    }
}
