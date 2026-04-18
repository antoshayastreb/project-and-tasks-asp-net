using ProjectTask.Domain.Common;

namespace ProjectTask.Domain.Entities;

/// <summary>
/// Сущность "Задача"(Task).
/// </summary>
public class ProjectTask : BaseEntity
{
    public ProjectTask (string title, string? description, Guid projectId, bool isCompleted = false)
    {
        Title = title;
        Description = description;
        ProjectId = projectId;
        IsCompleted = isCompleted;
    }

    private ProjectTask() { }
    
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Статус выполнения.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Проект.
    /// </summary>
    public Project? Project { get; private set; }
    
}
