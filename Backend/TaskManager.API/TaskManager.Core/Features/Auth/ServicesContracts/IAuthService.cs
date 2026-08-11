using TaskManager.Core.Common.Results;
using TaskManager.Core.Features.Auth.DTOs;

namespace TaskManager.Core.Features.Auth.ServicesContracts;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
}
