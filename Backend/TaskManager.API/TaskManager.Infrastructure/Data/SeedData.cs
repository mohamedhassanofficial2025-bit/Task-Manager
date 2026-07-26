using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;

namespace TaskManager.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(AddDbContext context)
    {
        if (!context.Projects.Any())
        {
            var project = new Project
            {
                Name = "Task Manager",
                Description = "Sample Project"
            };

            context.Projects.Add(project);

            context.TaskItems.Add(new TaskItem
            {
                Title = "Create API",
                Status = taskStatus.ToDo,
                Project = project
            });

            await context.SaveChangesAsync();
        }
    }
}