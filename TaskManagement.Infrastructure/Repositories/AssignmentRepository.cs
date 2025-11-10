using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly TaskManagementDbContext _context;
    
    public AssignmentRepository(TaskManagementDbContext context)
    {
        _context = context;
    }
    
    public async Task<Assignment> AddAsync(Assignment assignment)
    {
        await _context.Assignments.AddAsync(assignment);
        await _context.SaveChangesAsync();
        return assignment;
    }
    
    public async Task<bool> RemoveAsync(int taskItemId, int userId)
    {
        var assignment = await _context.Assignments
            .FirstOrDefaultAsync(a => a.TaskItemId == taskItemId && a.UserId == userId);
        
        if (assignment == null) return false;
        
        _context.Assignments.Remove(assignment);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<Assignment>> GetByTaskIdAsync(int taskItemId)
    {
        return await _context.Assignments
            .Where(a => a.TaskItemId == taskItemId)
            .Include(a => a.User)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Assignment>> GetByUserIdAsync(int userId)
    {
        return await _context.Assignments
            .Where(a => a.UserId == userId)
            .Include(a => a.TaskItem)
            .ToListAsync();
    }
}
