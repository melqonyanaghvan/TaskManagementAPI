using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetProjectWithTasksAsync(int projectId);
    Task<Project> AddAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
}
