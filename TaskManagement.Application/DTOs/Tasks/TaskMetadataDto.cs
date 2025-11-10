namespace TaskManagement.Application.DTOs.Tasks;

public class TaskMetadataDto
{
    public int EstimatedHours { get; set; }
    public string Tags { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
