using System.Text.Json;

namespace ToDoList.Api.Model.Dto;

public class OutboxResponseDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Published { get; set; }
    public object? Event { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string Type { get; set; } = string.Empty;

    public static OutboxResponseDto FromDomain(Outbox model)
    {
        return new OutboxResponseDto { 
            Id = model.Id, 
            CreatedAt = model.CreatedAt, 
            UpdatedAt = model.UpdatedAt,
            PublishedAt = model.PublishedAt, 
            Event = JsonSerializer.Deserialize<object>(model.Event),
            Published = model.Published,
            Type = model.Type
        };
    }
}
