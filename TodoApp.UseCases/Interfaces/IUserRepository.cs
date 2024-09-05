using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TodoApp.UseCases.Interfaces;


public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);

    Task<CreateUserResponseDTO?> AddUserAsync(CreateUserDTO user);

    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);

    Task<User?> GetByEmailAndPasswordAsync(string email, string password);
}
