using ToDoList.Api.Model.Dto;

namespace ToDoList.Api.Events;

public record ActivityStatusUpdatedEvent(Guid id, EnumDto status, string name) : IDomainEvent
{

}
