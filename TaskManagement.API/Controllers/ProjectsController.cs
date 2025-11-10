using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ITaskService _taskService;
    
    public ProjectsController(IProjectService projectService, ITaskService taskService)
    {
        _projectService = projectService;
        _taskService = taskService;
    }
    

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
            return NotFound(new { message = $"Project with ID {id} not found" });
        
        return Ok(project);
    }
    

    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> GetProjectTasks(int id)
    {
        var tasks = await _taskService.GetTasksByProjectIdAsync(id);
        return Ok(tasks);
    }
    

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto dto)
    {
        var project = await _projectService.CreateProjectAsync(dto);
        return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectCreateDto dto)
    {
        var project = await _projectService.UpdateProjectAsync(id, dto);
        if (project == null)
            return NotFound(new { message = $"Project with ID {id} not found" });
        
        return Ok(project);
    }
    

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        if (!result)
            return NotFound(new { message = $"Project with ID {id} not found" });
        
        return NoContent();
    }
}
