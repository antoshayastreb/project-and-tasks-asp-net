namespace ProjectManager.Application.Caching;

/// <summary>
/// Кэш-ключ сущностей
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// Кэш-ключ для проекта.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string ProjectById(Guid id) => $"project:{id}";

    /// <summary>
    /// Кэш-ключ для задачи.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string ProjectTaskById(Guid id) => $"project_task:{id}";
}
