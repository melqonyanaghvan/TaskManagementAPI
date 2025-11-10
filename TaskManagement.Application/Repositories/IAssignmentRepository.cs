using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Repositories;

public interface IAssignmentRepository
{
    Task<Assignment> AddAsync(Assignment assignment);
    Task<bool> RemoveAsync(int taskItemId, int userId);
    Task<IEnumerable<Assignment>> GetByTaskIdAsync(int taskItemId);
    Task<IEnumerable<Assignment>> GetByUserIdAsync(int userId);
}
