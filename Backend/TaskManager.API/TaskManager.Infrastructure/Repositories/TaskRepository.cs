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

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.TaskItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(int projectId)
    {
        return await _context.TaskItems
            .Where(t => t.ProjectId == projectId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetByStatusAsync(taskStatus status)
    {
        return await _context.TaskItems
            .Where(t => t.Status == status)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();
        return taskItem;
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem)
    {
        var existingTask = await _context.TaskItems.FindAsync(id);

        if (existingTask is null)
            return null;

        existingTask.Title = taskItem.Title;
        existingTask.Description = taskItem.Description;
        existingTask.Status = taskItem.Status;
        existingTask.DueDate = taskItem.DueDate;
        existingTask.ProjectId = taskItem.ProjectId;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var taskItem = await _context.TaskItems.FindAsync(id);

        if (taskItem is null)
            return false;

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
