using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Controllers;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Queries;
using ProjectManager.Application.Services;

namespace ProjectManager.Api.Tests.Controllers;

public class ProjectsControllerTests
{
    private readonly Mock<IProjectQueries> _queries = new Mock<IProjectQueries>();
    private readonly Mock<IProjectService> _service = new Mock<IProjectService>();

    private ProjectsController CreateController() => new ProjectsController(queries: _queries.Object, service: _service.Object);

    [Fact]
    public async Task Get_ReturnsOkWithDto_WhenProjectExists()
    {
        var projectId = Guid.NewGuid();
        var expected = new ProjectDto(
            Id: projectId,
            Name: "Test project",
            Description: "desc",
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: null,
            Tasks: Array.Empty<ProjectTaskListItemDto>()
        );

        _queries
            .Setup(q => q.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Get(projectId, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _queries.Verify(q => q.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        var projectId = Guid.NewGuid();
        var controller = CreateController();

        var result = await controller.Get(projectId, CancellationToken.None);
        
        var notFound = Assert.IsType<NotFoundResult>(result.Result);
        _queries.Verify(q => q.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsList_AndPassesPaginationParams()
    {
        const int page = 2;
        const int pageSize = 5;
        var expected = new List<ProjectListDto>
        {
            new(Guid.NewGuid(), "A", null, DateTime.UtcNow, null),
            new(Guid.NewGuid(), "B", "desc", DateTime.UtcNow, DateTime.UtcNow),
        };

        _queries
            .Setup(q => q.GetAllAsync(page, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.GetAll(page, pageSize, CancellationToken.None);

        Assert.Same(expected, result);
        _queries.Verify(q => q.GetAllAsync(page, pageSize, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsCreatedWithIdFromService()
    {
        var dto = new CreateProjectDto("New", "desc");
        var expectedId = Guid.NewGuid();

        _service
            .Setup(s => s.CreateAsync(It.IsAny<CreateProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var controller = CreateController();
        var result = await controller.Create(dto, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(expectedId, created.Value);
        _service.Verify(s => s.CreateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_PropagatesValidationException_FromService()
    {
        var dto = new CreateProjectDto("", null);
        var failures = new[] { new ValidationFailure("Name", "'Name' must not be empty.") };

        _service
            .Setup(s => s.CreateAsync(It.IsAny<CreateProjectDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(failures));

        var controller = CreateController();

        await Assert.ThrowsAsync<ValidationException>(
            () => controller.Create(dto, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ReturnsNoContent_AndCallsService()
    {
        var projectId = Guid.NewGuid();
        var dto = new UpdateProjectDto("Updated", null);
        var controller = CreateController();

        var result = await controller.Update(projectId, dto, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        _service.Verify(s => s.UpdateAsync(projectId, dto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_PropagatesNotFound_WhenServiceThrows()
    {
        var projectId = Guid.NewGuid();
        var dto = new UpdateProjectDto("Updated", null);

        _service
            .Setup(s => s.UpdateAsync(projectId, It.IsAny<UpdateProjectDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("Project", projectId));

        var controller = CreateController();

        await Assert.ThrowsAsync<NotFoundException>(
            () => controller.Update(projectId, dto, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_AndCallsService()
    {
        var projectId = Guid.NewGuid();
        var controller = CreateController();

        var result = await controller.Delete(projectId, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        _service.Verify(s => s.RemoveAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_PropagatesNotFound_WhenServiceThrows()
    {
        var projectId = Guid.NewGuid();

        _service
            .Setup(s => s.RemoveAsync(projectId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("Project", projectId));

        var controller = CreateController();

        await Assert.ThrowsAsync<NotFoundException>(
            () => controller.Delete(projectId, CancellationToken.None));
    }
}
