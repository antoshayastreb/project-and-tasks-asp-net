namespace ProjectManager.Domain.Common;

/// <summary>
/// Базовый класс доменной сущности.
/// </summary>
public abstract class BaseEntity
{

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего обновления.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }
}
