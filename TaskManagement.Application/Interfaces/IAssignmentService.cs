using TaskManagement.Application.DTOs.Assignments;

namespace TaskManagement.Application.Interfaces;

public interface IAssignmentService
{
    Task<AssignmentDto> AssignTaskToUserAsync(int taskItemId, int userId);
    Task<bool> UnassignTaskFromUserAsync(int taskItemId, int userId);
    Task<IEnumerable<AssignmentDto>> GetTaskAssignmentsAsync(int taskItemId);
    Task<IEnumerable<AssignmentDto>> GetUserAssignmentsAsync(int userId);
}
