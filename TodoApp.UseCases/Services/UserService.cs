using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.UseCases.Interfaces;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;
using System;

namespace TodoApp.UseCases.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<CreateUserResponseDTO> AddUserAsync(CreateUserDTO user)
    {
        var res = await _userRepository.AddUserAsync(user);
        if (res == null)
        {
            return null;
        }

        return new CreateUserResponseDTO
        {
            Id = res.Id,
            Name = res.Name,
            Email = res.Email
        };
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
    }

    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
    {
        // Note: In a real application, you should hash and validate the password securely.
        return await _userRepository.GetByEmailAndPasswordAsync(email, password);
    }
}