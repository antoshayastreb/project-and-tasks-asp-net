using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы projects
/// </summary>
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);
    }
}
