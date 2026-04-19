namespace ProjectManager.Application.DTOs.Project;

/// <summary>
/// DTO для возврата сущности Project
/// </summary>
public record ProjectDto (
    Guid Id, 
    string Name, 
    string? Description, 
    DateTime CreatedAt, 
    DateTime? UpdatedAt
);

/// <summary>
/// DTO для возврата сокращенной сущности Project для списка
/// </summary>
public record ProjectListDto (
    Guid Id, 
    string Name, 
    string? Description, 
    DateTime CreatedAt, 
    DateTime? UpdatedAt    
);

/// <summary>
/// DTO для создания сущности Project
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
public record CreateProjectDto (
    string Name,
    string? Description
);