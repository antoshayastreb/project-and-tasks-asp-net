using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence;

/// <summary>
/// Заполнение БД тестовыми данными для <see cref="AppDbContext"/>.
/// </summary>
public static class AppDbContextSeeder
{
    public static async Task SeedAsync(DbContext dbContext, bool _, CancellationToken ct)
    {
        var ctx = (AppDbContext)dbContext;

        if (await ctx.Projects.AnyAsync(ct))
        {
            return;
        }

        var website = new Project("Редизайн сайта", "Обновление главной страницы и каталога");
        website.AddTask("Согласовать макет", "Финальная версия с заказчиком", isCompleted: true);
        website.AddTask("Свёрстка главной", "HTML + CSS");
        website.AddTask("Интеграция с API", "Подключить endpoints каталога");

        var mobile = new Project("Мобильное приложение", "MVP iOS + Android");
        mobile.AddTask("Выбрать стек", "React Native или нативно");
        mobile.AddTask("Собрать команду", description: null);

        await ctx.Projects.AddRangeAsync(new[] { website, mobile }, ct);
        await ctx.SaveChangesAsync(ct);
    }
}
