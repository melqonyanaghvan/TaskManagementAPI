using TaskManagement.Application.DTOs.Assignments;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.Data;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly INotificationService _notificationService;  
    
    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        INotificationService notificationService) 
    {
        _assignmentRepository = assignmentRepository;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _notificationService = notificationService;
    }
    
    public async Task<AssignmentDto> AssignTaskToUserAsync(int taskItemId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var task = await _taskRepository.GetByIdAsync(taskItemId);
            if (task == null)
                throw new Exception($"Task with ID {taskItemId} not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            var assignment = new Assignment
            {
                TaskItemId = taskItemId,
                UserId = userId,
                AssignedAt = DateTime.UtcNow
            };
            
            await _assignmentRepository.AddAsync(assignment);
            
            user.Workload += 1;
            await _userRepository.UpdateAsync(user);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            await _notificationService.SendNotificationAsync(
                userId,
                $"Task '{task.Title}' has been assigned to you.",
                "Info");
            
            await _cacheService.RemoveAsync($"user_{userId}_tasks");
            
            return new AssignmentDto
            {
                TaskItemId = taskItemId,
                UserId = userId,
                Username = user.Username,
                TaskTitle = task.Title,
                AssignedAt = assignment.AssignedAt
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
    public async Task<bool> UnassignTaskFromUserAsync(int taskItemId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var result = await _assignmentRepository.RemoveAsync(taskItemId, userId);
            
            if (result)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null && user.Workload > 0)
                {
                    user.Workload -= 1;
                    await _userRepository.UpdateAsync(user);
                }
                
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                
                await _cacheService.RemoveAsync($"user_{userId}_tasks");
            }
            
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
    public async Task<IEnumerable<AssignmentDto>> GetTaskAssignmentsAsync(int taskItemId)
    {
        var assignments = await _assignmentRepository.GetByTaskIdAsync(taskItemId);
        
        return assignments.Select(a => new AssignmentDto
        {
            TaskItemId = a.TaskItemId,
            UserId = a.UserId,
            Username = a.User?.Username ?? "Unknown",
            TaskTitle = a.TaskItem?.Title ?? "Unknown",
            AssignedAt = a.AssignedAt
        });
    }
    
    public async Task<IEnumerable<AssignmentDto>> GetUserAssignmentsAsync(int userId)
    {
        var assignments = await _assignmentRepository.GetByUserIdAsync(userId);
        
        return assignments.Select(a => new AssignmentDto
        {
            TaskItemId = a.TaskItemId,
            UserId = a.UserId,
            Username = a.User?.Username ?? "Unknown",
            TaskTitle = a.TaskItem?.Title ?? "Unknown",
            AssignedAt = a.AssignedAt
        });
    }
}
