using System;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{

    private readonly AppDbContext _dbContext;

    public UnitOfWork(
        AppDbContext dbContext, 
        IProjectRepository projectRepository,
        IProjectTaskRepository projectTaskRepository
    )
    {
        _dbContext = dbContext;
        Projects = projectRepository;
        ProjectTasks = projectTaskRepository;
    }

    public IProjectRepository Projects { get; private set; }

    public IProjectTaskRepository ProjectTasks { get; private set; }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}
