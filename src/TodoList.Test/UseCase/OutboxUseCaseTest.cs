using AutoFixture;
using NSubstitute;
using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.Repository;
using ToDoList.Api.UseCase;

namespace TodoList.Test.UseCase;

[TestClass]
public class OutboxUseCaseTest
{
    private readonly OutboxUseCase _useCase;
    private readonly IOutboxRepository _repository;
    private readonly Fixture _fixture;

    public OutboxUseCaseTest()
    {
        _fixture = new();
        _repository = Substitute.For<IOutboxRepository>();
        _useCase = new (_repository);
    }

    [TestMethod]
    public async Task Should_Update_Execute()
    {
        var mockParam = _fixture.CreateMany<Outbox>();

        await _useCase.UpdateRange(mockParam, default);

        await _repository.Received(1).UpdateRange(mockParam, default);
    }

    [TestMethod]
    [DataRow(1, 20)]
    [DataRow(-1, 10)]
    [DataRow(2, -3)]
    public async Task Should_Get_Return_Ok(int page, int size)
    {
        var mockResponse = _fixture.CreateMany<Outbox>();

        _repository.Get(Arg.Any<Expression<Func<Outbox, bool>>?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.Get(c => c != null, page, size, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
    }
}
