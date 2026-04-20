using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities;

/// <summary>
/// Сущность "Проект".
/// </summary>
public class Project : BaseEntity
{

    public Project (string name, string? description)
    {
        Name = name;
        Description = description;
    }

    private Project () { }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; private set; }

    private readonly List<ProjectTask> _tasks = new();

    /// <summary>
    /// Список задач в проекте.
    /// </summary>
    public IReadOnlyCollection<ProjectTask> Tasks => _tasks.AsReadOnly();

    /// <summary>
    /// Создать задачу внутри проекта.
    /// </summary>
    /// <param name="title">Название задачи</param>
    /// <param name="description">Описание задачи</param>
    /// <param name="isCompleted">Статус задачи</param>
    /// <returns>Возвращает созданную задачу</returns>
    public ProjectTask AddTask(string title, string? description, bool isCompleted = false)
    {
        var task = new ProjectTask(
            title: title, 
            description: description, 
            projectId: this.Id, 
            isCompleted: isCompleted
        );
        _tasks.Add(task);
        return task;
    }

    /// <summary>
    /// Изменить название проекта.
    /// </summary>
    /// <param name="name"></param>
    public void ChangeName(string name)
    {
        this.Name = name;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Изменить описание проекта.
    /// </summary>
    /// <param name="description"></param>
    public void ChangeDescription(string? description)
    {
        this.Description = description;
        this.UpdatedAt = DateTime.UtcNow;
    }
}
