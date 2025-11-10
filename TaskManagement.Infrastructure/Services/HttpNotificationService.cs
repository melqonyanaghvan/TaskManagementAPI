using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public class HttpNotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpNotificationService> _logger;
    
    public HttpNotificationService(HttpClient httpClient, ILogger<HttpNotificationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task SendNotificationAsync(int userId, string message, string type = "Info")
    {
        try
        {
            var notification = new
            {
                userId,
                message,
                type
            };
            
            var response = await _httpClient.PostAsJsonAsync("/api/Notifications/send", notification);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Notification sent to user {UserId}: {Message}", userId, message);
            }
            else
            {
                _logger.LogWarning("Failed to send notification. Status: {Status}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
        }
    }
}
