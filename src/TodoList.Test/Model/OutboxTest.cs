using AutoFixture;
using ToDoList.Api.Model;

namespace TodoList.Test.Model;

[TestClass]
public class OutboxTest
{
    private readonly Fixture _fixture = new();

    [TestMethod]
    public void Shoud_Create_Outbox()
    {
        var @event = new { Code =  1 };
        var fake = _fixture.Create<Outbox>();
        fake.SetEvent(@event);

        var outbox = Outbox.Create(@event, fake.Type);

        Assert.IsNotNull(outbox);
        Assert.AreEqual(fake.Event, outbox.Event);
        Assert.IsNull(outbox.PublishedAt);
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    public void Should_Publish_Outbox(bool isPublished, bool expectedPublishedValue)
    {
        var outbox = _fixture.Create<Outbox>();
        outbox.SetPublished(isPublished);
        outbox.SetPublish();


        Assert.IsNotNull(outbox);
        Assert.AreEqual(expectedPublishedValue, outbox.Published);
    }
}
