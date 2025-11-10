using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Infrastructure.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(TaskManagementDbContext context) : base(context) { }
    
    public async Task<Project?> GetProjectWithTasksAsync(int projectId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
                .ThenInclude(t => t.Metadata)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }
}
