using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Core.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Navigation Property (One Project -> Many Tasks)
    public ICollection<TaskItem> Tasks { get; set; } = new HashSet<TaskItem>();
}
