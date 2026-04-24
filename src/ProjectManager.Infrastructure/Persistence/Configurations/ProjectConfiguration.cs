using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы projects
/// </summary>
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(m => m.Id);
        // Отключение генерации для pk
        // Генерация Id остается внутри приложения
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
