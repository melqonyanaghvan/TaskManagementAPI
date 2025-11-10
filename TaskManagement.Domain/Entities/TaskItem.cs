namespace TaskManagement.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; 
    public string Priority { get; set; } = "Medium"; 
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }
    
    public Project Project { get; set; } = null!;
    public TaskMetadata? Metadata { get; set; } 
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}
