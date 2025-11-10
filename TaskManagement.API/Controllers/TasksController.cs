using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IAssignmentService _assignmentService;
    
    public TasksController(ITaskService taskService, IAssignmentService assignmentService)
    {
        _taskService = taskService;
        _assignmentService = assignmentService;
    }
    

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound(new { message = $"Task with ID {id} not found" });
        
        return Ok(task);
    }
    

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredTasks(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 15,
        [FromQuery] string? status = null,
        [FromQuery] string? priority = null)
    {
        var tasks = await _taskService.GetFilteredTasksAsync(page, limit, status, priority);
        return Ok(tasks);
    }
    

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")] 
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
    {
        try
        {
            var task = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(id, dto);
            if (task == null)
                return NotFound(new { message = $"Task with ID {id} not found" });
            
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        if (!result)
            return NotFound(new { message = $"Task with ID {id} not found" });
        
        return NoContent();
    }

    [HttpPost("{taskId}/assign/{userId}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AssignTaskToUser(int taskId, int userId)
    {
        try
        {
            var assignment = await _assignmentService.AssignTaskToUserAsync(taskId, userId);
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{taskId}/unassign/{userId}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UnassignTaskFromUser(int taskId, int userId)
    {
        var result = await _assignmentService.UnassignTaskFromUserAsync(taskId, userId);
        if (!result)
            return NotFound(new { message = "Assignment not found" });
        
        return NoContent();
    }
    

    [HttpGet("{taskId}/assignments")]
    public async Task<IActionResult> GetTaskAssignments(int taskId)
    {
        var assignments = await _assignmentService.GetTaskAssignmentsAsync(taskId);
        return Ok(assignments);
    }
}
