using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities;

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

    /// <summary>
    /// Изменить название задачи.
    /// </summary>
    /// <param name="title"></param>
    public void ChangeTitle(string title)
    {
        this.Title = title;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Изменить описание задачи.
    /// </summary>
    /// <param name="description"></param>
    public void ChangeDescription(string? description)
    {
        this.Description = description;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Изменить статус выполнения задачи.
    /// </summary>
    /// <param name="isCompleted"></param>
    public void ChangeIsCompleted(bool isCompleted)
    {
        this.IsCompleted = isCompleted;
        this.UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Изменить проект задачи.
    /// </summary>
    /// <param name="id"></param>
    public void ChangeProjectId(Guid id)
    {
        this.ProjectId = id;
        this.UpdatedAt = DateTime.UtcNow;
    }
}
