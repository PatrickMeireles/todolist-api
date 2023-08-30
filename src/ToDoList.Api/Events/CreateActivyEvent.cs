using ToDoList.Api.Model;
using ToDoList.Api.Model.Dto;

namespace ToDoList.Api.Events;

public class CreateActivyEvent : IDomainEvent
{

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!; 
    public DateTime? DateEstimatedFinish { get; set; }
    public EnumDto Priority { get; set; } 
    public EnumDto Status { get; set; }

    public CreateActivyEvent(Guid id, string name, string description, DateTime? dateEstimatedFinish, EnumDto priority, EnumDto status)
    {
        Id = id;
        Name = name;
        Description = description;
        DateEstimatedFinish = dateEstimatedFinish;
        Priority = priority;
        Status = status;
    }
}
