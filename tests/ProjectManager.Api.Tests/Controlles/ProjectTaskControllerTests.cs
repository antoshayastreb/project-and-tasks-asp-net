using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Controllers;
using ProjectManager.Application.DTOs.ProjectTask;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Queries;
using ProjectManager.Application.Services;

namespace ProjectManager.Api.Tests.Controlles;

public class ProjectTaskControllerTests
{
    private readonly Mock<IProjectTaskQueries> _queries = new Mock<IProjectTaskQueries>();
    private readonly Mock<IProjectTaskService> _service = new Mock<IProjectTaskService>();

    private ProjectTaskController CreateController() => new ProjectTaskController(queries: _queries.Object, service: _service.Object);

    [Fact]
    public async Task Get_ReturnsOkWithDto_WhenProjectTaskExists()
    {
        var projectTaskId = Guid.NewGuid();
        var expected = new ProjectTaskDto(
            Id: projectTaskId,
            Title: "Test task",
            Description: null,
            IsCompleted: false,
            ProjectId: Guid.NewGuid(),
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: null
        );
        _queries
            .Setup(q => q.GetByIdAsync(projectTaskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var controller = CreateController();

        var result = await controller.Get(projectTaskId, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _queries.Verify(q => q.GetByIdAsync(projectTaskId, It.IsAny<CancellationToken>()), Times.Once);              
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenProjectTaskDoesNotExist()
    {
        var projectTaskId = Guid.NewGuid();
        var controller = CreateController();

        var result = await controller.Get(projectTaskId, CancellationToken.None);

        var notFound = Assert.IsType<NotFoundResult>(result.Result);
        _queries.Verify(q => q.GetByIdAsync(projectTaskId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsList_AndPassesFilters()
    {
        var projectId = Guid.NewGuid();
        bool? isCompleted = true;
        var expected = new List<ProjectTaskDto>
        {
            new(Guid.NewGuid(), "A", null, true, projectId, DateTime.UtcNow, null),
            new(Guid.NewGuid(), "B", "desc", true, projectId, DateTime.UtcNow, DateTime.UtcNow),
        };

        _queries
            .Setup(q => q.GetAllAsync(projectId, isCompleted, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.GetAll(projectId, isCompleted, CancellationToken.None);

        Assert.Same(expected, result);
        _queries.Verify(
            q => q.GetAllAsync(projectId, isCompleted, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAll_PassesNullFilters_WhenNotProvided()
    {
        var expected = new List<ProjectTaskDto>();

        _queries
            .Setup(q => q.GetAllAsync(null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.GetAll(null, null, CancellationToken.None);

        Assert.Same(expected, result);
        _queries.Verify(
            q => q.GetAllAsync(null, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsCreatedWithIdFromService()
    {
        var dto = new CreateProjectTaskDto("Title", "desc", Guid.NewGuid());
        var expectedId = Guid.NewGuid();

        _service
            .Setup(s => s.CreateAsync(It.IsAny<CreateProjectTaskDto>(), It.IsAny<CancellationToken>()))
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
        var dto = new CreateProjectTaskDto("", null, Guid.NewGuid());
        var failures = new[] { new ValidationFailure("Title", "'Title' must not be empty.") };

        _service
            .Setup(s => s.CreateAsync(It.IsAny<CreateProjectTaskDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(failures));

        var controller = CreateController();

        await Assert.ThrowsAsync<ValidationException>(
            () => controller.Create(dto, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ReturnsNoContent_AndCallsService()
    {
        var projectTaskId = Guid.NewGuid();
        var dto = new UpdateProjectTaskDto("Updated", null, null, true);
        var controller = CreateController();

        var result = await controller.Update(projectTaskId, dto, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        _service.Verify(
            s => s.UpdateAsync(projectTaskId, dto, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Update_PropagatesNotFound_WhenServiceThrows()
    {
        var projectTaskId = Guid.NewGuid();
        var dto = new UpdateProjectTaskDto("Updated", null, null, true);

        _service
            .Setup(s => s.UpdateAsync(projectTaskId, It.IsAny<UpdateProjectTaskDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("ProjectTask", projectTaskId));

        var controller = CreateController();

        await Assert.ThrowsAsync<NotFoundException>(
            () => controller.Update(projectTaskId, dto, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_AndCallsService()
    {
        var projectTaskId = Guid.NewGuid();
        var controller = CreateController();

        var result = await controller.Delete(projectTaskId, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        _service.Verify(s => s.RemoveAsync(projectTaskId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_PropagatesNotFound_WhenServiceThrows()
    {
        var projectTaskId = Guid.NewGuid();

        _service
            .Setup(s => s.RemoveAsync(projectTaskId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("ProjectTask", projectTaskId));

        var controller = CreateController();

        await Assert.ThrowsAsync<NotFoundException>(
            () => controller.Delete(projectTaskId, CancellationToken.None));
    }
}
