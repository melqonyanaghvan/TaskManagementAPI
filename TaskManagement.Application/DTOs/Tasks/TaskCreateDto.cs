namespace TaskManagement.Application.DTOs.Tasks;

public class TaskCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public int ProjectId { get; set; }
    public DateTime? Deadline { get; set; }
}
