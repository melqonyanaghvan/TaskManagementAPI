using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;  
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            Deadline = dto.Deadline,
            CreatedAt = DateTime.UtcNow
        };
        
        var created = await _projectRepository.AddAsync(project);
        
        return new ProjectDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            CreatedAt = created.CreatedAt,
            Deadline = created.Deadline,
            TaskCount = 0
        };
    }
    
    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetProjectWithTasksAsync(id);
        if (project == null) return null;
        
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            Deadline = project.Deadline,
            TaskCount = project.Tasks.Count
        };
    }
    
    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        
        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            Deadline = p.Deadline,
            TaskCount = p.Tasks.Count
        });
    }
    
    public async Task<ProjectDto?> UpdateProjectAsync(int id, ProjectCreateDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;
        
        project.Name = dto.Name;
        project.Description = dto.Description;
        project.Deadline = dto.Deadline;
        
        var updated = await _projectRepository.UpdateAsync(project);
        
        return new ProjectDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            CreatedAt = updated.CreatedAt,
            Deadline = updated.Deadline,
            TaskCount = updated.Tasks.Count
        };
    }
    
    public async Task<bool> DeleteProjectAsync(int id)
    {
        return await _projectRepository.DeleteAsync(id);
    }
}
