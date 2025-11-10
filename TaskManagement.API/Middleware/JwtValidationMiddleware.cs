using System.Security.Claims;

namespace TaskManagement.API.Middleware;

public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtValidationMiddleware> _logger;
    
    public JwtValidationMiddleware(RequestDelegate next, ILogger<JwtValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/auth"))
        {
            await _next(context);
            return;
        }
        
      
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Request to {Path} without JWT token", context.Request.Path);
        }
        else
        {
        
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
            
            _logger.LogInformation(
                "Request to {Path} by user ID: {UserId}, Email: {Email}, Role: {Role}",
                context.Request.Path, userId, userEmail, userRole);
        }
        
        await _next(context);
    }
}
