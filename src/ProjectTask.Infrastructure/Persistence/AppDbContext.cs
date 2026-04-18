using Microsoft.EntityFrameworkCore;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
