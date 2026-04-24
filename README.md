# Project & Task API

Веб-API на ASP.NET Core для управления проектами и задачами. Каждый проект содержит список задач, поддерживаются CRUD, пагинация, фильтрация, валидация, кэширование чтения и централизованная обработка ошибок.

## Стек

- **.NET 8**, ASP.NET Core Web API
- **EF Core 9** + **PostgreSQL 16** (Npgsql)
- **FluentValidation** — валидация входных DTO
- **Serilog** — логирование в консоль и файл
- **MemoryCache** — кэширование запросов чтения
- **Swagger (Swashbuckle)** — документация API
- **xUnit + Moq** — модульные тесты
- **Docker + docker compose** — контейнеризация

## Структура решения

```
src/
  ProjectManager.Api             — контроллеры, middleware, точка входа
  ProjectManager.Application     — интерфейсы сервисов/запросов, DTO, валидаторы
  ProjectManager.Domain          — доменные сущности (Project, ProjectTask)
  ProjectManager.Infrastructure  — EF Core, миграции, репозитории, сидинг
tests/
  ProjectManager.Api.Tests       — тесты контроллеров (xUnit + Moq)
```

Слои сверху вниз: `Api → Application → Domain`, а `Infrastructure` реализует интерфейсы `Domain`/`Application`.

## Запуск через docker compose (рекомендуется)

Требуется Docker и docker compose.

```bash
make up_service       # собирает образ API и поднимает API + PostgreSQL
# или без Makefile:
docker compose -f ./service.compose.yaml up -d --build
```

После старта:
- API: <http://localhost:8080>
- Swagger UI: <http://localhost:8080/swagger>

При первом старте EF Core применит миграции и наполнит БД тестовыми данными (через `UseAsyncSeeding`).

Остановить:

```bash
make down_service
# или:
docker compose -f ./service.compose.yaml down
```

## Локальный запуск (без докеризации API)

Нужна только PostgreSQL — можно поднять её из compose-файла:

```bash
make up_db            # поднимет только postgres на 5432
```

Затем в проекте API:

```bash
dotnet run --project src/ProjectManager.Api
```

По умолчанию приложение стартует на `http://localhost:5015` (Swagger: `/swagger`). Строка подключения берётся из `appsettings.json` / `appsettings.Development.json`.

## Тесты

```bash
dotnet test
```

## Логи

Пишутся в консоль и в файл `logs/log-YYYYMMDD.log` (Serilog, rolling by day). Настройка — в `appsettings.json`, секция `Serilog`.
