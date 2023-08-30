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
public class OutboxProcessBackgroundServicesTest
{
    private readonly OutboxProcessBackgroundServices _services;
    private readonly IOutboxUseCase _useCase;
    private readonly ILogger<OutboxProcessBackgroundServices> _logger;
    private readonly Fixture _fixture;

    public OutboxProcessBackgroundServicesTest()
    {
        _useCase = Substitute.For<IOutboxUseCase>();
        _logger = Substitute.For<ILogger<OutboxProcessBackgroundServices>>();
        _services = new OutboxProcessBackgroundServices(_useCase, _logger);
        _fixture = new();
    }

    [TestMethod]
    public async Task Should_Execute_Service_When_There_Is_Not_Items_To_Process()
    {
        var mockResponse = new List<Outbox>();

        _useCase.Get(Arg.Any<Expression<Func<Outbox, bool>>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var mockParam = Substitute.For<IJobExecutionContext>();

        await _services.Execute(mockParam);

        await _useCase.Received(0).UpdateRange(Arg.Any<IEnumerable<Outbox>>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Should_Execute_Service()
    {
        var mockResponse = _fixture.Build<Outbox>()
            .CreateMany();

        _useCase.Get(Arg.Any<Expression<Func<Outbox, bool>>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var mockParam = Substitute.For<IJobExecutionContext>();

        await _services.Execute(mockParam);

        await _useCase.Received(1).UpdateRange(Arg.Any<IEnumerable<Outbox>>(), Arg.Any<CancellationToken>());
    }
}
