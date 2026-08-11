using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Enums;
using TaskManager.Core.Features.Tasks.DTOs;
using TaskManager.Core.Features.Tasks.ServicesContracts;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>
    /// Retrieves all tasks for the authenticated user.
    /// Route: GET /api/Tasks
    /// Requirement: Full CRUD for Tasks.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _taskService.GetAllAsync(GetUserId());
        if (!result.IsSuccess)
            return Ok(result);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific task by its ID (owned by the authenticated user).
    /// Route: GET /api/Tasks/{id} (e.g., /api/Tasks/1)
    /// Requirement: Full CRUD for Tasks.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _taskService.GetByIdAsync(id, GetUserId());
        if (!result.IsSuccess)
            return Ok(result);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all tasks for a specific project (owned by the authenticated user).
    /// Route: GET /api/Tasks/project/{id}/tasks (e.g., /api/Tasks/project/1/tasks)
    /// Requirement: Endpoint to get all tasks for a specific project.
    /// </summary>
    [HttpGet("project/{id}/tasks")]
    public async Task<IActionResult> GetByProjectId(int id)
    {
        var result = await _taskService.GetAllByProjectIdAsync(id, GetUserId());
        if (!result.IsSuccess)
            return Ok(result);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all tasks filtered by a specific status (owned by the authenticated user).
    /// Route: GET /api/Tasks/status/{status} (e.g., /api/Tasks/status/ToDo)
    /// Requirement: Endpoint to filter tasks by Status.
    /// </summary>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(taskStatus status)
    {
        var result = await _taskService.GetByStatusAsync(status, GetUserId());
        if (!result.IsSuccess)
            return Ok(result);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new task (for a project owned by the authenticated user).
    /// Route: POST /api/Tasks
    /// Requirement: Full CRUD for Tasks.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var result = await _taskService.CreateAsync(dto, GetUserId());
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Updates an existing task completely (owned by the authenticated user).
    /// Route: PUT /api/Tasks/{id} (e.g., /api/Tasks/1)
    /// Requirement: Full CRUD for Tasks.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var result = await _taskService.UpdateAsync(id, dto, GetUserId());
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Partially updates the status of a task (owned by the authenticated user).
    /// Route: PATCH /api/Tasks/{id}/status (e.g., /api/Tasks/1/status)
    /// Requirement: Change a task's status directly.
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusDto dto)
    {
        var result = await _taskService.UpdateStatusAsync(id, dto, GetUserId());
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a task by its ID (owned by the authenticated user).
    /// Route: DELETE /api/Tasks/{id} (e.g., /api/Tasks/1)
    /// Requirement: Full CRUD for Tasks.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteAsync(id, GetUserId());
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
