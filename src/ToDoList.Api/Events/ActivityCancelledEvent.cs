using ToDoList.Api.Model.Dto;

namespace ToDoList.Api.Events;

public record ActivityCancelledEvent(Guid id, EnumDto status, string name) : IDomainEvent
{
}
