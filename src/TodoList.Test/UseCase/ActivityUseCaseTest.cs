using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToDoList.Api.Model;
using ToDoList.Api.Repository;
using ToDoList.Api.UseCase;

namespace TodoList.Test.UseCase;

[TestClass]
public class ActivityUseCaseTest
{
    private readonly IActivityRepository _repository;
    private readonly ActivityUseCase _useCase;
    private Fixture _fixture;
    public ActivityUseCaseTest()
    {
        _repository = Substitute.For<IActivityRepository>();
        _useCase = new(_repository);
        _fixture = new();
    }

    [TestMethod]
    public async Task Should_GetById_Return_Ok()
    {
        var mockResponse = _fixture.Create<Activity>();

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.GetById(mockResponse.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(mockResponse.Id, result.Id);
    }

    [TestMethod]
    public async Task Should_GetById_Return_Null()
    {
        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _useCase.GetById(Guid.NewGuid());

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task Should_Add_Return_True()
    {
        var mockParam = _fixture.Create<Activity>();

        _repository.Add(Arg.Any<Activity>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _useCase.Add(mockParam);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task Should_Add_Return_False()
    {
        var mockParam = _fixture.Create<Activity>();

        _repository.Add(Arg.Any<Activity>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _useCase.Add(mockParam);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task Should_Next_Status_Return_Null()
    {

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _useCase.NextStatus(Guid.NewGuid(), default);

        Assert.IsNull(result);

        await _repository.Received(0).Update(Arg.Any<Activity>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    [DataRow(Status.Finished)]
    [DataRow(Status.Canceled)]
    public async Task Should_Next_Status_Return_False(Status param)
    {
        var mockResponse = _fixture.Create<Activity>();

        mockResponse.SetStatus(param);

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.NextStatus(mockResponse.Id, default);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Value);

        await _repository.Received(0).Update(Arg.Any<Activity>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    [DataRow(Status.Created)]
    [DataRow(Status.InProgress)]
    public async Task Should_Next_Status_Return_True(Status param)
    {
        var mockResponse = _fixture.Create<Activity>();

        mockResponse.SetStatus(param);

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.NextStatus(mockResponse.Id, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Value);

        await _repository.Received(1).Update(Arg.Any<Activity>(), Arg.Any<CancellationToken>());
    }


    [TestMethod]
    public async Task Should_Cancel_Status_Return_Null()
    {

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _useCase.Cancel(Guid.NewGuid(), default);

        Assert.IsNull(result);

        await _repository.Received(0).Update(Arg.Any<Activity>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    [DataRow(Status.Finished)]
    [DataRow(Status.Canceled)]
    public async Task Should_Cancel_Status_Return_False(Status param)
    {
        var mockResponse = _fixture.Create<Activity>();

        mockResponse.SetStatus(param);

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.Cancel(mockResponse.Id, default);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Value);

        await _repository.Received(0).Update(Arg.Any<Activity>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    [DataRow(Status.Created)]
    [DataRow(Status.InProgress)]
    public async Task Should_Cancel_Status_Return_True(Status param)
    {
        var mockResponse = _fixture.Create<Activity>();

        mockResponse.SetStatus(param);

        _repository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.Cancel(mockResponse.Id, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Value);

        await _repository.Received(1).Update(Arg.Any<Activity>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Should_UpdateRange_Execute()
    {
        var mockParam = _fixture.CreateMany<Activity>();

        await _useCase.UpdateRange(mockParam);

        await _repository.Received(1).UpdateRange(Arg.Any<IEnumerable<Activity>>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    [DataRow(1, 20)]
    [DataRow(-1, 10)]
    [DataRow(2, -3)]
    public async Task Should_Get_Return_Ok(int page, int size)
    {
        var mockResponse = _fixture.CreateMany<Activity>();

        _repository.Get(Arg.Any<Expression<Func<Activity, bool>>?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(mockResponse);

        var result = await _useCase.Get(c => c != null, page, size, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
    }

    [TestMethod]
    public async Task Should_Count_Execute()
    {
        var mockValue = _fixture.Create<int>();

        _useCase.Count(Arg.Any<Expression<Func<Activity, bool>>?>(), Arg.Any<CancellationToken>()).Returns(mockValue);

        var result = await _useCase.Count();

        Assert.AreEqual(mockValue, result);
    }
}
