using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Common.Results;
using TaskManager.Core.Features.Admin.DTOs;
using TaskManager.Core.Features.Admin.ServicesContracts;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public AdminService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "User",
                IsActive = !await _userManager.IsLockedOutAsync(user)
            });
        }

        return Result<IEnumerable<UserResponseDto>>.Success(userDtos);
    }

    public async Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return Result<UserResponseDto>.Failure("A user with this email already exists.");

        if (!await _roleManager.RoleExistsAsync(dto.Role))
            return Result<UserResponseDto>.Failure($"Role '{dto.Role}' does not exist. Valid roles are: Admin, User.");

        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<UserResponseDto>.Failure($"User creation failed: {errors}");
        }

        await _userManager.AddToRoleAsync(user, dto.Role);

        return Result<UserResponseDto>.Success(new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Role = dto.Role,
            IsActive = true
        }, "User created successfully.");
    }

    public async Task<Result<UserResponseDto>> ChangeRoleAsync(string userId, ChangeRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result<UserResponseDto>.Failure("User not found.");

        if (!await _roleManager.RoleExistsAsync(dto.Role))
            return Result<UserResponseDto>.Failure($"Role '{dto.Role}' does not exist. Valid roles are: Admin, User.");

        // Remove all existing roles and assign the new one
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, dto.Role);

        return Result<UserResponseDto>.Success(new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Role = dto.Role,
            IsActive = !await _userManager.IsLockedOutAsync(user)
        }, "User role updated successfully.");
    }

    public async Task<Result> ToggleUserStatusAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure("User not found.");

        if (await _userManager.IsLockedOutAsync(user))
        {
            // Enable user: remove lockout
            await _userManager.SetLockoutEndDateAsync(user, null);
            return Result.Success("User has been enabled.");
        }
        else
        {
            // Disable user: lock out indefinitely
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return Result.Success("User has been disabled.");
        }
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure("User not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure($"Failed to delete user: {errors}");
        }

        return Result.Success("User deleted successfully.");
    }
}
