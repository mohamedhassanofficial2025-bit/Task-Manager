using TaskManager.Core.Common.Results;
using TaskManager.Core.Enums;
using TaskManager.Core.Features.Tasks.DTOs;

namespace TaskManager.Core.Features.Tasks.ServicesContracts;

public interface ITaskService
{
    Task<Result<IEnumerable<TaskResponseDto>>> GetAllAsync();
    Task<Result<IEnumerable<TaskResponseDto>>> GetAllByProjectIdAsync(int projectId);
    Task<Result<IEnumerable<TaskResponseDto>>> GetByStatusAsync(taskStatus status);
    Task<Result<TaskResponseDto>> GetByIdAsync(int id);
    Task<Result<TaskResponseDto>> CreateAsync(CreateTaskDto dto);
    Task<Result<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto);
    Task<Result<TaskResponseDto>> UpdateStatusAsync(int id, UpdateTaskStatusDto dto);
    Task<Result> DeleteAsync(int id);
}
