using System.Text.Json;

namespace ToDoList.Api.Model;

public class Outbox : Entity
{
    public bool Published { get; protected set; }
    public string Event { get; protected set; } = string.Empty;
    public DateTime? PublishedAt { get; protected set; }
    public string Type { get; protected set; } = string.Empty;

    public Outbox(Guid id) : base(id)
    {
    }

    protected Outbox(Guid id, object @event, string type) : base(id)
    {
        Event = JsonSerializer.Serialize(@event, new JsonSerializerOptions());
        Type = type;
    }

    public static Outbox Create(object @event, string type)
    {
        return new(Guid.NewGuid(), @event, type);
    }

    public void SetPublish()
    {
        if(!Published)
        {
            Published = true;
            PublishedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }

    public void SetPublished(bool published) =>
        Published = published;

    public void SetEvent(object @event) =>
        Event = JsonSerializer.Serialize(@event, new JsonSerializerOptions());
}
