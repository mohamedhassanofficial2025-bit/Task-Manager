using TaskManager.Core.Enums;
namespace TaskManager.Core.Features.Tasks.DTOs;
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public taskStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }
}