using TaskManager.Core.Common.Results;
using TaskManager.Core.Features.Admin.DTOs;

namespace TaskManager.Core.Features.Admin.ServicesContracts;

public interface IAdminService
{
    Task<Result<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
    Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto);
    Task<Result<UserResponseDto>> ChangeRoleAsync(string userId, ChangeRoleDto dto);
    Task<Result> ToggleUserStatusAsync(string userId);
    Task<Result> DeleteUserAsync(string userId);
}
