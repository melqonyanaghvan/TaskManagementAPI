namespace TaskManagement.Domain.Entities;

public class TaskMetadata
{
    public int Id { get; set; }
    public int TaskItemId { get; set; } 
    public int EstimatedHours { get; set; }
    public string Tags { get; set; } = string.Empty; 
    public string Notes { get; set; } = string.Empty;
    
    public TaskItem TaskItem { get; set; } = null!;
}
