using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;


public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);

    Task<CreateUserResponseDTO?> AddUserAsync(CreateUserDTO user);

    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}
