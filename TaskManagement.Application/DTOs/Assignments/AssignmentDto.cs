namespace TaskManagement.Application.DTOs.Assignments;

public class AssignmentDto
{
    public int TaskItemId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string TaskTitle { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
}
