using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Api.Model;

namespace TodoList.Test.Model;

[TestClass]
public class ActivityTest
{
    private readonly Fixture _fixture = new();

    [TestMethod]
    public void Should_Create_Activity()
    {
        var fake = _fixture.Create<Activity>();

        var activity = Activity.Create(fake.Name, fake.Description, fake.DateEstimatedFinish, fake.Priority);

        Assert.IsNotNull(activity);
        Assert.AreEqual(fake.Name, activity.Name);
        Assert.AreEqual(fake.Description, activity.Description);
        Assert.AreEqual(fake.DateEstimatedFinish, activity.DateEstimatedFinish);
        Assert.AreEqual(fake.Priority, activity.Priority);
        Assert.AreEqual(Status.Created, activity.Status);
        Assert.IsTrue(activity.DomainEvents.Any());
    }

    [TestMethod]
    [DataRow(Status.Created, Status.InProgress, true)]
    [DataRow(Status.InProgress, Status.Finished, true)]
    [DataRow(Status.Finished, Status.Finished, false)]
    [DataRow(Status.Canceled, Status.Canceled, false)]
    public void Should_Activity_Next_Status(Status actual, Status expected, bool updatedAtHasValue)
    {
        var activity = _fixture.Create<Activity>();
        activity.SetStatus(actual);

        activity.NextStatus();

        Assert.IsNotNull(activity);
        Assert.AreEqual(expected, activity.Status);
        Assert.AreEqual(updatedAtHasValue, activity.UpdatedAt.HasValue);
        Assert.IsTrue(activity.DomainEvents.Any());
    }

    [TestMethod]
    [DataRow(Status.Created, Status.Canceled, true)]
    [DataRow(Status.InProgress, Status.Canceled, true)]
    [DataRow(Status.Finished, Status.Finished, false)]
    [DataRow(Status.Canceled, Status.Canceled, false)]
    public void Should_Activity_Cancel_Status(Status actual, Status expected, bool updatedAtHasValue)
    {
        var activity = _fixture.Create<Activity>();
        activity.SetStatus(actual);

        activity.CancelStatus();

        Assert.IsNotNull(activity);
        Assert.AreEqual(expected, activity.Status);
        Assert.AreEqual(updatedAtHasValue, activity.UpdatedAt.HasValue);
        Assert.IsTrue(activity.DomainEvents.Any());
    }

    [TestMethod]
    [DataRow(true, true, false, false)]
    [DataRow(false, true, true, true)]
    public void Should_Activity_Delayed(bool delayed, bool expected, bool updatedAtHasValue, bool eventHasValue)
    {
        var activity = _fixture.Create<Activity>();
        activity.SetDelayed(delayed);

        activity.IsDelay();

        Assert.IsNotNull(activity);
        Assert.AreEqual(expected, activity.Delayed);
        Assert.AreEqual(updatedAtHasValue, activity.UpdatedAt.HasValue);
        Assert.AreEqual(eventHasValue, activity.DomainEvents.Any());

    }
}
