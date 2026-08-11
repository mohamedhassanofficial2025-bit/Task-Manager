using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Features.Tasks.RepositoryContracts;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AddDbContext _context;

    public TaskRepository(AddDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(string userId)
    {
        return await _context.TaskItems
            .Include(t => t.Project)
            .Where(t => t.Project!.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(int projectId, string userId)
    {
        return await _context.TaskItems
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId && t.Project!.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetByStatusAsync(taskStatus status, string userId)
    {
        return await _context.TaskItems
            .Include(t => t.Project)
            .Where(t => t.Status == status && t.Project!.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id, string userId)
    {
        return await _context.TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId);
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem, string userId)
    {
        // Verify the target project belongs to the user
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == taskItem.ProjectId && p.UserId == userId);

        if (!projectExists)
            throw new UnauthorizedAccessException("You do not have access to the specified project.");

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();
        return taskItem;
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem, string userId)
    {
        var existingTask = await _context.TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId);

        if (existingTask is null)
            return null;

        existingTask.Title = taskItem.Title;
        existingTask.Description = taskItem.Description;
        existingTask.DueDate = taskItem.DueDate;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var taskItem = await _context.TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId);

        if (taskItem is null)
            return false;

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
