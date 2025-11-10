using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<IEnumerable<TaskItem>> GetTasksWithMetadataAsync();
    Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId);
    Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId); 
    Task<IEnumerable<TaskItem>> GetFilteredTasksAsync(int page, int limit, string? status, string? priority);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
}
