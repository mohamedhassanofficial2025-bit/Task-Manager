using AutoMapper;
using FluentValidation;
using TaskManager.Core.Common.Results;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Features.Tasks.DTOs;
using TaskManager.Core.Features.Tasks.RepositoryContracts;
using TaskManager.Core.Features.Tasks.ServicesContracts;

namespace TaskManager.Core.Features.Tasks.Services;

public class TaskService : ITaskService
{
    /*---------------------------------------------------------------*/
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTaskDto> _createValidator;
    private readonly IValidator<UpdateTaskDto> _updateValidator;
    private readonly IValidator<UpdateTaskStatusDto> _updateStatusValidator;

    public TaskService(
        ITaskRepository repository, 
        IMapper mapper, 
        IValidator<CreateTaskDto> createValidator, 
        IValidator<UpdateTaskDto> updateValidator,
        IValidator<UpdateTaskStatusDto> updateStatusValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _updateStatusValidator = updateStatusValidator;
    }

    /*---------------------------------------------------------------*/
    /*---------------------------------------------------------------*/
    public async Task<Result<IEnumerable<TaskResponseDto>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        if (result == null || !result.Any())
            return Result<IEnumerable<TaskResponseDto>>.Failure("No tasks found.");
        return Result<IEnumerable<TaskResponseDto>>.Success(_mapper.Map<IEnumerable<TaskResponseDto>>(result));
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<IEnumerable<TaskResponseDto>>> GetAllByProjectIdAsync(int projectId)
    {
        var result = await _repository.GetAllByProjectIdAsync(projectId);
        if (result == null || !result.Any())
            return Result<IEnumerable<TaskResponseDto>>.Failure($"No tasks found for Project Id {projectId}.");
        return Result<IEnumerable<TaskResponseDto>>.Success(_mapper.Map<IEnumerable<TaskResponseDto>>(result));
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<IEnumerable<TaskResponseDto>>> GetByStatusAsync(taskStatus status)
    {
        var result = await _repository.GetByStatusAsync(status);
        if (result == null || !result.Any())
            return Result<IEnumerable<TaskResponseDto>>.Failure($"No tasks found with status {status}.");
        return Result<IEnumerable<TaskResponseDto>>.Success(_mapper.Map<IEnumerable<TaskResponseDto>>(result));
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<TaskResponseDto>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
            return Result<TaskResponseDto>.Failure("Task not found.");
        return Result<TaskResponseDto>.Success(_mapper.Map<TaskResponseDto>(result));
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<TaskResponseDto>> CreateAsync(CreateTaskDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<TaskResponseDto>.Failure($"Validation failed: {errors}");
        }

        var task = _mapper.Map<TaskItem>(dto);
        var createdTask = await _repository.CreateAsync(task);
        
        return Result<TaskResponseDto>.Success(_mapper.Map<TaskResponseDto>(createdTask), "Task created successfully.");
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<TaskResponseDto>> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<TaskResponseDto>.Failure($"Validation failed: {errors}");
        }

        var taskToUpdate = _mapper.Map<TaskItem>(dto);
        var updatedTask = await _repository.UpdateAsync(id, taskToUpdate);
        
        if (updatedTask == null)
            return Result<TaskResponseDto>.Failure("Task not found.");
            
        return Result<TaskResponseDto>.Success(_mapper.Map<TaskResponseDto>(updatedTask), "Task updated successfully.");
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<TaskResponseDto>> UpdateStatusAsync(int id, UpdateTaskStatusDto dto)
    {
        var validationResult = await _updateStatusValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<TaskResponseDto>.Failure($"Validation failed: {errors}");
        }

        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
            return Result<TaskResponseDto>.Failure("Task not found.");

        existingTask.Status = dto.Status;
        
        // Use existing Update method since it handles saving
        var updatedTask = await _repository.UpdateAsync(id, existingTask);
        
        return Result<TaskResponseDto>.Success(_mapper.Map<TaskResponseDto>(updatedTask), "Task status updated successfully.");
    }
    /*---------------------------------------------------------------*/

    public async Task<Result> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
            return Result.Failure("Task not found or could not be deleted.");
        return Result.Success("Task deleted successfully.");
    }
    /*---------------------------------------------------------------*/
}
