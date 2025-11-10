using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.Services;   

namespace TaskManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ICacheService _cacheService;
    
    public UserService(
        IUserRepository userRepository, 
        ITaskRepository taskRepository,
        ICacheService cacheService)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _cacheService = cacheService;
    }
    
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Workload = user.Workload,
            CreatedAt = user.CreatedAt
        };
    }
    
    public async Task<UserDto?> GetUserProfileAsync(int userId)
    {
        return await GetUserByIdAsync(userId);
    }
    
    public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(int userId)
    {
       
        var cacheKey = $"user_{userId}_tasks";
        var cached = await _cacheService.GetAsync<IEnumerable<TaskDto>>(cacheKey);
        
        if (cached != null)
            return cached;
        
       
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            ProjectId = t.ProjectId,
            ProjectName = t.Project?.Name ?? "Unknown",
            CreatedAt = t.CreatedAt,
            Deadline = t.Deadline
        }).ToList();
        
      
        await _cacheService.SetAsync(cacheKey, taskDtos, TimeSpan.FromMinutes(10));
        
        return taskDtos;
    }
    
    public async Task<int> GetUserWorkloadAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user?.Workload ?? 0;
    }
}
