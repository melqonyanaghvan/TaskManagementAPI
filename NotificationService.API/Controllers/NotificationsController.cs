using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Models;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private static readonly List<NotificationRequest> _notifications = new();
    private readonly ILogger<NotificationsController> _logger;
    
    public NotificationsController(ILogger<NotificationsController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost("send")]
    public IActionResult SendNotification([FromBody] NotificationRequest notification)
    {
        _logger.LogInformation(
            "Notification sent to User {UserId}: {Message} (Type: {Type})",
            notification.UserId,
            notification.Message,
            notification.Type);
        
        _notifications.Add(notification);
        
        return Ok(new
        {
            success = true,
            message = "Notification sent successfully",
            notificationId = _notifications.Count,
            timestamp = notification.Timestamp
        });
    }
    

    [HttpGet("user/{userId}")]
    public IActionResult GetUserNotifications(int userId)
    {
        var userNotifications = _notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.Timestamp)
            .ToList();
        
        return Ok(new
        {
            userId,
            count = userNotifications.Count,
            notifications = userNotifications
        });
    }
    
 
    [HttpGet("all")]
    public IActionResult GetAllNotifications()
    {
        return Ok(new
        {
            total = _notifications.Count,
            notifications = _notifications.OrderByDescending(n => n.Timestamp).ToList()
        });
    }
}
