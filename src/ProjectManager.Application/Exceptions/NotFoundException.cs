namespace ProjectManager.Application.Exceptions;

/// <summary>
/// Ошибка для случая отсутствия ресурса. 
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string resource, object id) 
        : base($"{resource} with id `{id}` was not found") { }
}
