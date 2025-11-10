using TaskManagement.Application.DTOs.Projects;

namespace TaskManagement.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(ProjectCreateDto dto);
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> UpdateProjectAsync(int id, ProjectCreateDto dto);
    Task<bool> DeleteProjectAsync(int id);
}
