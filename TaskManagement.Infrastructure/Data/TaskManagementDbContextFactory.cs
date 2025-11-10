using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagement.Infrastructure.Data;

public class TaskManagementDbContextFactory : IDesignTimeDbContextFactory<TaskManagementDbContext>
{
    public TaskManagementDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskManagementDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=taskmanagement;Username=postgres;Password=postgres");
        
        return new TaskManagementDbContext(optionsBuilder.Options);
    }
}
