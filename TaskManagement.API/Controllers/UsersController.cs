using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAssignmentService _assignmentService;
    
    public UsersController(IUserService userService, IAssignmentService assignmentService)
    {
        _userService = userService;
        _assignmentService = assignmentService;
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(int id)
    {
        var user = await _userService.GetUserProfileAsync(id);
        if (user == null)
            return NotFound(new { message = $"User with ID {id} not found" });
        
        return Ok(user);
    }

    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> GetUserTasks(int id)
    {
        var tasks = await _userService.GetUserTasksAsync(id);
        return Ok(tasks);
    }
    

    [HttpGet("{id}/workload")]
    public async Task<IActionResult> GetUserWorkload(int id)
    {
        var workload = await _userService.GetUserWorkloadAsync(id);
        return Ok(new { userId = id, workload });
    }
    
    [HttpGet("{id}/assignments")]
    public async Task<IActionResult> GetUserAssignments(int id)
    {
        var assignments = await _assignmentService.GetUserAssignmentsAsync(id);
        return Ok(assignments);
    }
}
