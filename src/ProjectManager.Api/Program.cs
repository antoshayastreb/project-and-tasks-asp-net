using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.Caching;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.Queries;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Repositories;
using ProjectManager.Infrastructure;
using ProjectManager.Infrastructure.Persistence;
using ProjectManager.Infrastructure.Persistence.Repositories;
using ProjectManager.Infrastructure.Queries;
using Serilog;

Log.Logger  = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    builder.Services.AddControllers(); 
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);        
    });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));

    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<ICacheInvalidator, CacheInvalidator>();
    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
    builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
    builder.Services.AddScoped<ProjectQueries>();
    builder.Services.AddScoped<IProjectQueries>(sp => 
        new CachedProjectQueries(
            innerQueries: sp.GetRequiredService<ProjectQueries>(),
            cache: sp.GetRequiredService<IMemoryCache>()
        )
    );
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
    builder.Services.AddScoped<ProjectTaskQueries>();
    builder.Services.AddScoped<IProjectTaskQueries>(sp => 
        new CachedProjectTaskQueries(
            innerQueries: sp.GetRequiredService<ProjectTaskQueries>(),
            cache: sp.GetRequiredService<IMemoryCache>()
        )
    );
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectDto>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler();
    app.UseSerilogRequestLogging(); 
    app.UseHttpsRedirection();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    app.Run();
}
catch (Exception exc)
{
    Log.Fatal(exc, "Unexpected error");
}
finally
{
    Log.CloseAndFlush();
}
