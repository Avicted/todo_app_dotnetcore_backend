using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // User not found
            return null;
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        if (result == PasswordVerificationResult.Success)
        {
            // Password is correct
            return user;
        }

        // Password is incorrect
        return null;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            return new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        return null;
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