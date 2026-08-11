using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Features.Auth.DTOs;
using TaskManager.Core.Features.Auth.ServicesContracts;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account.
    /// Route: POST /api/Account/register
    /// Creates a standard "User" account by default.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// Route: POST /api/Account/login
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }
}
