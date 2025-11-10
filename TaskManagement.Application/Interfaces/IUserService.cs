using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserProfileAsync(int userId);
    Task<IEnumerable<TaskDto>> GetUserTasksAsync(int userId);
    Task<int> GetUserWorkloadAsync(int userId);
}
