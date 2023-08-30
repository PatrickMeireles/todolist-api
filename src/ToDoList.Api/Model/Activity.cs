using ToDoList.Api.Events;
using ToDoList.Api.Model.Dto;

namespace ToDoList.Api.Model;

public class Activity : AggregateRoot
{
    public string Name { get; protected set; } = null!;

    public string Description { get; protected set; } = null!;

    public DateTime? DateEstimatedFinish { get; protected set; }

    public Priority Priority { get; protected set; }

    public Status Status { get; protected set; }

    public bool Delayed { get; protected set; } = false;

    public Activity(Guid id) : base(id)
    {
    }

    protected Activity(Guid id, string name, string description, DateTime? dateEstimatedFinish, Priority priority, Status status) : base(id)
    {
        Name = name;
        Description = description;
        DateEstimatedFinish = dateEstimatedFinish;
        Priority = priority;
        Status = status;
    }

    public static Activity Create(string name, string description, DateTime? dateEstimatedFinish, Priority priority)
    {
        var status = Status.Created;

        var activity = new Activity(Guid.NewGuid(), name, description, dateEstimatedFinish, priority, status);

        var @event = new CreateActivyEvent(activity.Id, name, description, dateEstimatedFinish, new EnumDto(priority), new EnumDto(status));

        activity.AddEvent(@event);

        return activity;
    }

    public void NextStatus()
    {
        switch (Status)
        {
            case Status.Created:
                Status = Status.InProgress;
                UpdatedAt = DateTime.Now;
                break;

            case Status.InProgress:
                Status = Status.Finished;
                UpdatedAt = DateTime.Now;
                break;

            case Status.Finished:
                break;
        }

        var @event = new ActivityStatusUpdatedEvent(Id, new EnumDto(Status), Name);

        AddEvent(@event);
    }

    public void CancelStatus()
    {
        switch (Status)
        {
            case Status.Created:
            case Status.InProgress:
                Status = Status.Canceled;
                UpdatedAt = DateTime.Now;
                break;
            default:
                break;
        }

        var @event = new ActivityCancelledEvent(Id, new EnumDto(Status), Name);

        AddEvent(@event);
    }

    public void IsDelay()
    {
        if (!Delayed)
        {
            Delayed = true;
            UpdatedAt = DateTime.Now;
            var @event = new ActivityDelayedEvent(Id, Name, DateEstimatedFinish, new EnumDto(Status));

            AddEvent(@event);
        }
    }

    public void SetDelayed(bool delayed) =>
        Delayed = delayed;

    public void SetStatus(Status status) =>
        Status = status;
}

public enum Priority
{
    Low,
    Medium,
    High
}

public enum Status
{
    Created,
    InProgress,
    Finished,
    Canceled
}