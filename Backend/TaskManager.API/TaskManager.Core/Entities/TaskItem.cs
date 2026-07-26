using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public taskStatus Status { get; set; } = taskStatus.ToDo;

    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }

    // Navigation Property
    public Project? Project { get; set; }
}