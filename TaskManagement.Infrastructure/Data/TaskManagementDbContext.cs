using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data;

public class TaskManagementDbContext : DbContext
{
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options)
    {
    }
    
    
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskMetadata> TaskMetadata { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Metadata)
            .WithOne(m => m.TaskItem)
            .HasForeignKey<TaskMetadata>(m => m.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Assignment>()
            .HasKey(a => new { a.TaskItemId, a.UserId }); 
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.TaskItem)
            .WithMany(t => t.Assignments)
            .HasForeignKey(a => a.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Assignments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.Status);
        
        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.Priority);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
