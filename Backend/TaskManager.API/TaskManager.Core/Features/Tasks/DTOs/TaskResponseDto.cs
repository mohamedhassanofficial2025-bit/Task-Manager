using TaskManager.Core.Enums;
namespace TaskManager.Core.Features.Tasks.DTOs;
public class TaskResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public taskStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }
}