using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Features.Projects.DTOs;
using TaskManager.Core.Features.Projects.ServicesContracts;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Retrieves all projects.
    /// Route: GET /api/Projects
    /// Requirement: Full CRUD for Projects.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _projectService.GetAllAsync();
        if (!result.IsSuccess)
            return Ok(result);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific project by its ID.
    /// Route: GET /api/Projects/{id} (e.g., /api/Projects/1)
    /// Requirement: Full CRUD for Projects.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _projectService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new project.
    /// Route: POST /api/Projects
    /// Requirement: Full CRUD for Projects.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var result = await _projectService.CreateAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Updates an existing project.
    /// Route: PUT /api/Projects/{id} (e.g., /api/Projects/1)
    /// Requirement: Full CRUD for Projects.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var result = await _projectService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a project by its ID.
    /// Route: DELETE /api/Projects/{id} (e.g., /api/Projects/1)
    /// Requirement: Full CRUD for Projects.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(id);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
