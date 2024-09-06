#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;

namespace TodoApp.UseCases.Interfaces;

public interface ITodoItemRepository
{
    Task<TodoItem?> GetByIdAsync(int id);
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<CreateTodoItemResponseDTO> AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
}