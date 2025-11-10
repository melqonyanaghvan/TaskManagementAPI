namespace TaskManagement.Application.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(int userId, string message, string type = "Info");
}
