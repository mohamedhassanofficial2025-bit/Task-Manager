using TaskManager.Core.Entities;

namespace TaskManager.Core.Features.Projects.RepositoryContracts;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync(string userId);
    Task<Project?> GetByIdAsync(int id, string userId);
    Task<Project> CreateAsync(Project project);
    Task<Project?> UpdateAsync(int id, Project project, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}
