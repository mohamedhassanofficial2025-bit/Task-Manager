using TaskManager.Core.Enums;

namespace TaskManager.Core.Features.Tasks.DTOs;
public class UpdateTaskStatusDto
{
    public taskStatus Status { get; set; }
}