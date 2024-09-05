using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<CreateUserResponseDTO?> AddUserAsync(CreateUserDTO user)
    {
        // Check if user already exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return null; // Return nothing
        }

        // Create a password hasher instance
        var passwordHasher = new PasswordHasher<User>();

        // Create new user and hash the password
        var newUser = new User
        {
            Name = user.Name,
            Email = user.Email,
        };

        // Hash the password
        newUser.Password = passwordHasher.HashPassword(newUser, user.Password);

        // Add user to the database
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new CreateUserResponseDTO
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Email = newUser.Email
        };
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}