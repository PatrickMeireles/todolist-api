namespace ToDoList.Api.Model.Dto;

public class ActivityResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DateEstimatedFinish { get; set; }
    public EnumDto? Priority { get; set; }
    public EnumDto? Status { get; set; }
    public bool Delayed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ActivityResponseDto FromDomain(Activity model)
    {
        return new()
        {
            CreatedAt = model.CreatedAt,
            DateEstimatedFinish = model.DateEstimatedFinish,
            Description = model.Description,
            Id = model.Id,
            Name = model.Name,
            Priority = new EnumDto(model.Priority),
            Status = new EnumDto(model.Status),
            Delayed = model.Delayed,
            UpdatedAt = model.UpdatedAt
        };
    }

}
