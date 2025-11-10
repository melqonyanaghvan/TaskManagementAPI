namespace TaskManagement.Application.DTOs.Tasks;

public class TaskUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; 
    public string Priority { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
}
