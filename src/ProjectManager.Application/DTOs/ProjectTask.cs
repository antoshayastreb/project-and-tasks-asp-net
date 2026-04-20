namespace ProjectManager.Application.DTOs.ProjectTask;

/// <summary>
/// DTO для возврата сущности ProjectTask.
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="IsCompleted"></param>
/// <param name="ProjectId"></param>
/// <param name="CreatedAt"></param>
/// <param name="UpdatedAt"></param>
public record ProjectTaskDto (
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    Guid ProjectId,
    DateTime CreatedAt, 
    DateTime? UpdatedAt
);

/// <summary>
/// DTO для возврата списочного элемента ProjectTask.
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="IsCompleted"></param>
/// <param name="CreatedAt"></param>
/// <param name="UpdatedAt"></param>
public record ProjectTaskListItemDto (
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime CreatedAt, 
    DateTime? UpdatedAt
);

/// <summary>
/// DTO для создания сущности ProjectTask.
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="ProjectId"></param>
/// <param name="IsCompleted"></param>
public record CreateProjectTaskDto (
    string Title,
    string? Description,
    Guid ProjectId,
    bool IsCompleted = false
);

/// <summary>
/// DTO для обновления сущности ProjectTask.
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="ProjectId"></param>
/// <param name="IsCompleted"></param>
public record UpdateProjectTaskDto (
    string? Title,
    string? Description,
    Guid? ProjectId,
    bool? IsCompleted
);
