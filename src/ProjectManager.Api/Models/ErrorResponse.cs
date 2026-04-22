namespace ProjectManager.Api.Models;

/// <summary>
/// Описание ответа при возникновении ошибки.
/// </summary>
/// <param name="Error">Название ошибки(краткое описание)</param>
/// <param name="Detail">Детализация по ошибке</param>
public record ErrorResponse(string Error, object Detail);
