using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : Repository<TaskItem>, ITaskRepository
{
    public TaskRepository(TaskManagementDbContext context) : base(context) { }
    
    public async Task<IEnumerable<TaskItem>> GetTasksWithMetadataAsync()
    {
        return await _context.Tasks
            .Include(t => t.Metadata)
            .Include(t => t.Project)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Metadata)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId)
    {
        return await _context.Tasks
            .Where(t => t.Assignments.Any(a => a.UserId == userId))
            .Include(t => t.Metadata)
            .Include(t => t.Project)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<TaskItem>> GetFilteredTasksAsync(
        int page, 
        int limit, 
        string? status, 
        string? priority)
    {
        var query = _context.Tasks.AsQueryable();
        
        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);
        
        if (!string.IsNullOrEmpty(priority))
            query = query.Where(t => t.Priority == priority);
        
        return await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .Include(t => t.Metadata)
            .ToListAsync();
    }
}
