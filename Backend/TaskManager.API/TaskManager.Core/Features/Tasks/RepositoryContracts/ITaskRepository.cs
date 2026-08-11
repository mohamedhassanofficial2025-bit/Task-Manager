using TaskManager.Core.Entities;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Features.Tasks.RepositoryContracts;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync(string userId);
    Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(int projectId, string userId);
    Task<IEnumerable<TaskItem>> GetByStatusAsync(taskStatus status, string userId);
    Task<TaskItem?> GetByIdAsync(int id, string userId);
    Task<TaskItem> CreateAsync(TaskItem taskItem, string userId);
    Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}
