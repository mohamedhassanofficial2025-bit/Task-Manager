using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Features.Projects.RepositoryContracts;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AddDbContext _context;

    public ProjectRepository(AddDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(string userId)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id, string userId)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> UpdateAsync(int id, Project project, string userId)
    {
        var existingProject = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (existingProject is null)
            return null;

        existingProject.Name = project.Name;
        existingProject.Description = project.Description;

        await _context.SaveChangesAsync();
        return existingProject;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (project is null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
}
