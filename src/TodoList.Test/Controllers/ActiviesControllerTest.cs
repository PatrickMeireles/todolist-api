using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Api.Controllers;
using ToDoList.Api.Model;
using ToDoList.Api.Model.Dto;
using ToDoList.Api.UseCase;

namespace TodoList.Test.Controllers;

[TestClass]
public class ActiviesControllerTest
{
    private readonly ActiviesController _controller;
    private readonly IActivityUseCase _activityUseCase;
    private Fixture _fixture;

    public ActiviesControllerTest()
    {
        _activityUseCase = Substitute.For<IActivityUseCase>();
        _controller = new(_activityUseCase);
        _fixture = new();
    }

    [TestMethod]
    public async Task Should_GetById_Return_Not_Found()
    {
        _activityUseCase.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(NotFoundResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_GetById_Return_Ok()
    {
        var mockResponse = _fixture.Create<Activity>();

        _activityUseCase.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _controller.GetById(mockResponse.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Post_Return_BadRequest()
    {
        var mockParam = _fixture.Create<ActivyRequestDto>();

        _activityUseCase.Add(Arg.Any<Activity>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _controller.Post(mockParam);

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(BadRequestResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Post_Return_Created()
    {
        var mockParam = _fixture.Create<ActivyRequestDto>();

        _activityUseCase.Add(Arg.Any<Activity>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _controller.Post(mockParam);

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(CreatedResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Update_Status_Return_Not_Found()
    {
        _activityUseCase.NextStatus(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _controller.NextStatus(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(NotFoundResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Update_Status_Return_BadRequest()
    {
        _activityUseCase.NextStatus(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _controller.NextStatus(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Update_Status_Return_Ok()
    {
        _activityUseCase.NextStatus(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _controller.NextStatus(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Cancel_Status_Return_Not_Found()
    {
        _activityUseCase.Cancel(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _controller.Cancel(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(NotFoundResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Cancel_Status_Return_BadRequest()
    {
        _activityUseCase.Cancel(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _controller.Cancel(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
    }

    [TestMethod]
    public async Task Should_Cancel_Status_Return_Ok()
    {
        _activityUseCase.Cancel(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _controller.Cancel(Guid.NewGuid());

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkResult), result.GetType());
    }
}
