using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Quartz;
using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.Services;
using ToDoList.Api.UseCase;

namespace TodoList.Test.Services;

[TestClass]
public class TasksDelaysBackgroundServicesTest
{
    private readonly TasksDelaysBackgroundServices _services;
    private readonly IActivityUseCase _activityUseCase;
    private readonly ILogger<TasksDelaysBackgroundServices> _logger;
    private readonly Fixture _fixture;

    public TasksDelaysBackgroundServicesTest()
    {
        _activityUseCase = Substitute.For<IActivityUseCase>();
        _logger = Substitute.For<ILogger<TasksDelaysBackgroundServices>>();
        _services = new(_activityUseCase, _logger);
        _fixture = new Fixture();
    }

    [TestMethod]
    public async Task Should_Execute_Service()
    {
        var mockResponse = _fixture.CreateMany<Activity>();

        _activityUseCase.Count(Arg.Any<Expression<Func<Activity, bool>>>(), Arg.Any<CancellationToken>()).Returns(mockResponse.Count());

        _activityUseCase.Get(Arg.Any<Expression<Func<Activity, bool>>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var mockParam = Substitute.For<IJobExecutionContext>();

        await _services.Execute(mockParam);

        await _activityUseCase.Received(1).Count(Arg.Any<Expression<Func<Activity, bool>>>(), Arg.Any<CancellationToken>());

        await _activityUseCase.Received().UpdateRange(Arg.Any<IEnumerable<Activity>>(), Arg.Any<CancellationToken>());
    }
}
