using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(TaskCreateDto dto);
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> UpdateTaskAsync(int id, TaskUpdateDto dto);
    Task<bool> DeleteTaskAsync(int id);
    Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId);
    Task<PagedResult<TaskDto>> GetFilteredTasksAsync(int page, int limit, string? status, string? priority);
}
