using TaskManager.Core.Common.Results;
using TaskManager.Core.Enums;
using TaskManager.Core.Features.Tasks.DTOs;

namespace TaskManager.Core.Features.Tasks.ServicesContracts;

public interface ITaskService
{
    Task<Result<IEnumerable<TaskResponseDto>>> GetAllAsync(string userId);
    Task<Result<IEnumerable<TaskResponseDto>>> GetAllByProjectIdAsync(int projectId, string userId);
    Task<Result<IEnumerable<TaskResponseDto>>> GetByStatusAsync(taskStatus status, string userId);
    Task<Result<TaskResponseDto>> GetByIdAsync(int id, string userId);
    Task<Result<TaskResponseDto>> CreateAsync(CreateTaskDto dto, string userId);
    Task<Result<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto, string userId);
    Task<Result<TaskResponseDto>> UpdateStatusAsync(int id, UpdateTaskStatusDto dto, string userId);
    Task<Result> DeleteAsync(int id, string userId);
}
