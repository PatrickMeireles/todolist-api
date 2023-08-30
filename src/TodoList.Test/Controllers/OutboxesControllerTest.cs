using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Linq.Expressions;
using ToDoList.Api.Controllers;
using ToDoList.Api.Model;
using ToDoList.Api.UseCase;

namespace TodoList.Test.Controllers;

[TestClass]
public class OutboxesControllerTest
{
    private OutboxesController _controller;
    private IOutboxUseCase _useCase;
    private Fixture _fixture;

    public OutboxesControllerTest()
    {
        _fixture = new();
        _useCase = Substitute.For<IOutboxUseCase>();
        _controller = new OutboxesController(_useCase);
    }

    [TestMethod]
    public async Task Should_Get_Return_Ok()
    {
        var mockResponse = _fixture.CreateMany<Outbox>();

        _useCase.Get(Arg.Any<Expression<Func<Outbox, bool>>?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _controller.Get();

        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
    }
}
