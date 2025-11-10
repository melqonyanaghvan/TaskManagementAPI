using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;  
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    
    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }
    
    public async Task<TaskDto> CreateTaskAsync(TaskCreateDto dto)
    {
        
        var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
        if (project == null)
            throw new Exception($"Project with ID {dto.ProjectId} not found");

        if (dto.Deadline.HasValue && dto.Deadline.Value < DateTime.UtcNow)
            throw new Exception("Deadline cannot be in the past");
        
        if (!TaskPriority.IsValid(dto.Priority))
            throw new Exception($"Invalid priority. Allowed: {string.Join(", ", TaskPriority.All)}");
        
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            ProjectId = dto.ProjectId,
            Deadline = dto.Deadline,
            Status = Domain.Enums.TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        var created = await _taskRepository.AddAsync(task);
        
        return MapToDto(created, project.Name);
    }
    
    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return null;
        
        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        return MapToDto(task, project?.Name ?? "Unknown");
    }
    
    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetTasksWithMetadataAsync();
        var result = new List<TaskDto>();
        
        foreach (var task in tasks)
        {
            var project = await _projectRepository.GetByIdAsync(task.ProjectId);
            result.Add(MapToDto(task, project?.Name ?? "Unknown"));
        }
        
        return result;
    }
    
    public async Task<TaskDto?> UpdateTaskAsync(int id, TaskUpdateDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return null;
        
        // Валидация
        if (!string.IsNullOrEmpty(dto.Status) && !Domain.Enums.TaskStatus.IsValid(dto.Status))
            throw new Exception($"Invalid status. Allowed: {string.Join(", ", Domain.Enums.TaskStatus.All)}");
        
        if (!string.IsNullOrEmpty(dto.Priority) && !TaskPriority.IsValid(dto.Priority))
            throw new Exception($"Invalid priority. Allowed: {string.Join(", ", TaskPriority.All)}");
        
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.Deadline = dto.Deadline;
        
        var updated = await _taskRepository.UpdateAsync(task);
        var project = await _projectRepository.GetByIdAsync(updated.ProjectId);
        
        return MapToDto(updated, project?.Name ?? "Unknown");
    }
    
    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _taskRepository.DeleteAsync(id);
    }
    
    public async Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId)
    {
        var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);
        var project = await _projectRepository.GetByIdAsync(projectId);
        
        return tasks.Select(t => MapToDto(t, project?.Name ?? "Unknown"));
    }
    
    public async Task<PagedResult<TaskDto>> GetFilteredTasksAsync(int page, int limit, string? status, string? priority)
    {
        var tasks = await _taskRepository.GetFilteredTasksAsync(page, limit, status, priority);
        var allTasks = await _taskRepository.GetAllAsync();
        
        var filteredCount = allTasks.Count();
        if (!string.IsNullOrEmpty(status))
            filteredCount = allTasks.Count(t => t.Status == status);
        if (!string.IsNullOrEmpty(priority))
            filteredCount = allTasks.Count(t => t.Priority == priority);
        
        var result = new List<TaskDto>();
        foreach (var task in tasks)
        {
            var project = await _projectRepository.GetByIdAsync(task.ProjectId);
            result.Add(MapToDto(task, project?.Name ?? "Unknown"));
        }
        
        return new PagedResult<TaskDto>
        {
            Data = result,
            Page = page,
            Limit = limit,
            TotalCount = filteredCount
        };
    }
    
    private TaskDto MapToDto(TaskItem task, string projectName)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            ProjectId = task.ProjectId,
            ProjectName = projectName,
            CreatedAt = task.CreatedAt,
            Deadline = task.Deadline,
            Metadata = task.Metadata != null ? new TaskMetadataDto
            {
                EstimatedHours = task.Metadata.EstimatedHours,
                Tags = task.Metadata.Tags,
                Notes = task.Metadata.Notes
            } : null
        };
    }
}
