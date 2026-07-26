
using TaskManager.Core.Common.Results;
using TaskManager.Core.Features.Projects.DTOs;
using TaskManager.Core.Features.Projects.ServicesContracts;
using TaskManager.Core.Features.Projects.RepositoryContracts;
using AutoMapper;
using FluentValidation;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Features.Projects.Services;

public class ProjectService : IProjectService
{
    /*---------------------------------------------------------------*/
    private readonly IProjectRepository  _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectService(IProjectRepository repository, IMapper mapper, IValidator<CreateProjectDto> createValidator, IValidator<UpdateProjectDto> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /*---------------------------------------------------------------*/
    /*---------------------------------------------------------------*/
    public async Task<Result<IEnumerable<ProjectResponseDto>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        if (result == null || !result.Any())
            return Result<IEnumerable<ProjectResponseDto>>.Failure("No projects found.");
        return Result<IEnumerable<ProjectResponseDto>>.Success(_mapper.Map<IEnumerable<ProjectResponseDto>>(result));
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<ProjectResponseDto>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
            return Result<ProjectResponseDto>.Failure("Project not found.");
        return Result<ProjectResponseDto>.Success(_mapper.Map<ProjectResponseDto>(result));
    }
    /*---------------------------------------------------------------*/
    public async Task<Result<ProjectResponseDto>> CreateAsync(CreateProjectDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<ProjectResponseDto>.Failure($"Validation failed: {errors}");
        }

        var project = _mapper.Map<Project>(dto);
        var createdProject = await _repository.CreateAsync(project);
        
        return Result<ProjectResponseDto>.Success(_mapper.Map<ProjectResponseDto>(createdProject), "Project created successfully.");
    }
    /*---------------------------------------------------------------*/

    public async Task<Result<ProjectResponseDto>> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<ProjectResponseDto>.Failure($"Validation failed: {errors}");
        }

        var projectToUpdate = _mapper.Map<Project>(dto);
        var updatedProject = await _repository.UpdateAsync(id, projectToUpdate);
        
        if (updatedProject == null)
            return Result<ProjectResponseDto>.Failure("Project not found.");
            
        return Result<ProjectResponseDto>.Success(_mapper.Map<ProjectResponseDto>(updatedProject), "Project updated successfully.");
    }
    /*---------------------------------------------------------------*/
    public async Task<Result> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
            return Result.Failure("Project not found or could not be deleted.");
        return Result.Success("Project deleted successfully.");
    }
    /*---------------------------------------------------------------*/

}
