using ProjectManager.Application.DTOs.Projects;

namespace ProjectManager.Application.Queries;

public interface IProjectQueries
{
    /// <summary>
    /// Возвращает проект со всеми задачами по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Возвращает список проектов с пагинацией.
    /// </summary>
    /// <param name="page">Номер страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ProjectListDto>> GetAllAsync(int page, int pageSize, CancellationToken ct);
}
