using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Queries;
using ProjectManager.Infrastructure.Persistence;

namespace ProjectManager.Infrastructure.Queries;

public class ProjectQueries : IProjectQueries
{
    private readonly AppDbContext _dbContext;

    public ProjectQueries(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Projects.AsNoTracking()
            .Include(p => p.Tasks)
            .Where(p => p.Id == id)
            .Select(p => new ProjectDto(
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<ProjectListDto>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _dbContext.Projects.AsNoTracking()
            .OrderBy(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectListDto(
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt
            ))
            .ToListAsync(ct);
    }
}
