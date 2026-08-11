using TaskManager.Core.Common.Results;
using TaskManager.Core.Features.Projects.DTOs;

namespace TaskManager.Core.Features.Projects.ServicesContracts;

public interface IProjectService
{
    Task<Result<IEnumerable<ProjectResponseDto>>> GetAllAsync(string userId);
    Task<Result<ProjectResponseDto>> GetByIdAsync(int id, string userId);
    Task<Result<ProjectResponseDto>> CreateAsync(CreateProjectDto dto, string userId);
    Task<Result<ProjectResponseDto>> UpdateAsync(int id, UpdateProjectDto dto, string userId);
    Task<Result> DeleteAsync(int id, string userId);
}
