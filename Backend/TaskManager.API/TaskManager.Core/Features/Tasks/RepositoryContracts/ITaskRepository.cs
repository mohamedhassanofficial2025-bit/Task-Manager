using TaskManager.Core.Entities;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Features.Tasks.RepositoryContracts;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(int projectId);
    Task<IEnumerable<TaskItem>> GetByStatusAsync(taskStatus status);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(TaskItem taskItem);
    Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem);
    Task<bool> DeleteAsync(int id);
}
