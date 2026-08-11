using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Features.Admin.DTOs;
using TaskManager.Core.Features.Admin.ServicesContracts;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Retrieves all users.
    /// Route: GET /api/Admin/users
    /// Requirement: Admin only.
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _adminService.GetAllUsersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user account.
    /// Route: POST /api/Admin/users
    /// Requirement: Admin only.
    /// </summary>
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var result = await _adminService.CreateUserAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Changes a user's role.
    /// Route: PUT /api/Admin/users/{id}/role
    /// Requirement: Admin only.
    /// </summary>
    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> ChangeRole(string id, [FromBody] ChangeRoleDto dto)
    {
        var result = await _adminService.ChangeRoleAsync(id, dto);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Toggles a user's active/disabled status.
    /// Route: PUT /api/Admin/users/{id}/toggle-status
    /// Requirement: Admin only.
    /// </summary>
    [HttpPut("users/{id}/toggle-status")]
    public async Task<IActionResult> ToggleUserStatus(string id)
    {
        var result = await _adminService.ToggleUserStatusAsync(id);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a user.
    /// Route: DELETE /api/Admin/users/{id}
    /// Requirement: Admin only.
    /// </summary>
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _adminService.DeleteUserAsync(id);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
