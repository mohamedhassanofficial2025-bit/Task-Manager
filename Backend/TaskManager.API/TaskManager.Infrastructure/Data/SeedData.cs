using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;

namespace TaskManager.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AddDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // --- Seed Roles ---
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole
                {
                    Name = roleName,
                    Description = roleName == "Admin"
                        ? "Administrator with user-management permissions"
                        : "Standard user with project and task management permissions"
                });
            }
        }

        // --- Seed Admin User ---
        const string adminEmail = "admin@taskflow.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "TaskFlow",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // --- Seed Demo User + Projects + Tasks ---
        if (!context.Projects.Any())
        {
            const string demoEmail = "demo@taskflow.com";
            var demoUser = await userManager.FindByEmailAsync(demoEmail);
            if (demoUser == null)
            {
                demoUser = new AppUser
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    FirstName = "Demo",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(demoUser, "Demo@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(demoUser, "User");
                }
            }

            var projects = new List<Project>
            {
                new Project { Name = "E-Commerce Website", Description = "Build a full-stack online store.", CreatedAt = DateTime.UtcNow, UserId = demoUser!.Id },
                new Project { Name = "HR Management System", Description = "Internal tool for HR department.", CreatedAt = DateTime.UtcNow, UserId = demoUser.Id },
                new Project { Name = "Mobile App Refactoring", Description = "Refactor the existing iOS app to React Native.", CreatedAt = DateTime.UtcNow, UserId = demoUser.Id },
                new Project { Name = "Data Migration", Description = "Migrate legacy data to the new cloud database.", CreatedAt = DateTime.UtcNow, UserId = demoUser.Id },
                new Project { Name = "Marketing Campaign Q3", Description = "Assets and tracking for Q3 marketing.", CreatedAt = DateTime.UtcNow, UserId = demoUser.Id }
            };

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();

            var tasks = new List<TaskItem>
            {
                // Project 1 Tasks
                new TaskItem { Title = "Design DB Schema", Description = "Create ERD for E-Commerce", Status = taskStatus.Done, ProjectId = projects[0].Id, DueDate = DateTime.UtcNow.AddDays(2) },
                new TaskItem { Title = "Implement Cart", Description = "Add to cart logic", Status = taskStatus.InProgress, ProjectId = projects[0].Id, DueDate = DateTime.UtcNow.AddDays(5) },
                
                // Project 2 Tasks
                new TaskItem { Title = "Employee Onboarding Flow", Description = "UI for onboarding", Status = taskStatus.ToDo, ProjectId = projects[1].Id, DueDate = DateTime.UtcNow.AddDays(10) },
                
                // Project 3 Tasks
                new TaskItem { Title = "Setup React Native CLI", Description = "Initialize new project", Status = taskStatus.Done, ProjectId = projects[2].Id },
                new TaskItem { Title = "Migrate Login Screen", Description = "Convert Swift to TS", Status = taskStatus.InProgress, ProjectId = projects[2].Id, DueDate = DateTime.UtcNow.AddDays(3) },
                
                // Project 4 Tasks
                new TaskItem { Title = "Extract Data", Description = "SQL Scripts for extraction", Status = taskStatus.Done, ProjectId = projects[3].Id },
                new TaskItem { Title = "Transform Data", Description = "Data cleaning", Status = taskStatus.ToDo, ProjectId = projects[3].Id, DueDate = DateTime.UtcNow.AddDays(7) },
                
                // Project 5 Tasks
                new TaskItem { Title = "Create Banner Ads", Description = "Design 3 ad creatives", Status = taskStatus.InProgress, ProjectId = projects[4].Id, DueDate = DateTime.UtcNow.AddDays(4) },
                new TaskItem { Title = "Setup Google Analytics", Description = "Add tracking codes", Status = taskStatus.ToDo, ProjectId = projects[4].Id }
            };

            await context.TaskItems.AddRangeAsync(tasks);
            await context.SaveChangesAsync();
        }
    }
}