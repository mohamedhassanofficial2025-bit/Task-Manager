using TaskManager.Core.Common.Results;
using TaskManager.Core.Features.Projects.DTOs;

namespace TaskManager.Core.Features.Projects.ServicesContracts;

public interface IProjectService
{
    Task<Result<IEnumerable<ProjectResponseDto>>> GetAllAsync();
    Task<Result<ProjectResponseDto>> GetByIdAsync(int id);
    Task<Result<ProjectResponseDto>> CreateAsync(CreateProjectDto dto);
    Task<Result<ProjectResponseDto>> UpdateAsync(int id, UpdateProjectDto dto);
    Task<Result> DeleteAsync(int id);
}
