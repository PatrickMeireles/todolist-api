namespace ToDoList.Api.Model;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; protected set; }

    public Entity(Guid id) =>
        Id = id;

    public Entity(Guid id, DateTime updatedAt)
    {
        Id = id;
        UpdatedAt = updatedAt;
    }
}
