using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы project_tasks
/// </summary>
public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("project_tasks");

        builder.HasKey(m => m.Id);
        // Отключение генерации для pk
        // Генерация Id остается внутри приложения
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);
    }
}
