using ToDoList.Api.Model.Dto;

namespace ToDoList.Api.Events;

public record ActivityDelayedEvent(Guid id, string name, DateTime? dateEstimatedFinish, EnumDto status) : IDomainEvent
{
}
