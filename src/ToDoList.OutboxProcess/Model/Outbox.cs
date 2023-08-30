using System;

namespace ToDoList.OutboxProcess.Model;

public class Outbox
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Published { get; set; }
    public object Event { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public string Type { get; set; } = string.Empty;
}
