namespace ProjectManager.Application.Caching;

/// <summary>
/// Сервис инвалидации кэша.
/// </summary>
public interface ICacheInvalidator
{
    /// <summary>
    /// Инвалидирует кэш по ключу.
    /// </summary>
    /// <param name="key"></param>
    void InvalidateCache(object key);
}
