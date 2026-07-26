namespace TaskManager.Core.Features.Tasks.DTOs;

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}