namespace NotificationService.API.Models;

public class NotificationRequest
{
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info"; 
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
