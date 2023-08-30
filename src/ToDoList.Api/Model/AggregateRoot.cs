using ToDoList.Api.Events;

namespace ToDoList.Api.Model;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    protected void AddEvent(IDomainEvent domainEvent)
    {
        if(domainEvent is not null)
            _events.Add(domainEvent);
    }

    protected AggregateRoot(Guid id) : base(id)
    {
    }

    public virtual void ClearEvents()
    {
        _events.Clear();
    }
}
