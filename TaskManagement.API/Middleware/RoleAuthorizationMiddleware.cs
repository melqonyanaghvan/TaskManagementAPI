using System.Security.Claims;

namespace TaskManagement.API.Middleware;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RoleAuthorizationMiddleware> _logger;
    
    public RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {

        if (context.Request.Path.StartsWithSegments("/api/auth") || 
            context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }
        
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      
            _logger.LogInformation(
                "User {UserId} with role {Role} accessing {Path}",
                userId, userRole, context.Request.Path);
            
            if (context.Request.Method == "DELETE" && userRole != "Admin")
            {
                _logger.LogWarning(
                    "User {UserId} with role {Role} tried to DELETE {Path} - Access denied",
                    userId, userRole, context.Request.Path);
                
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new 
                { 
                    message = "Access denied. Only Admins can delete resources." 
                });
                return;
            }
        }
        
        await _next(context);
    }
}
