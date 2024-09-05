using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface ITodoItemRepository
{
    Task<TodoItem> GetByIdAsync(int id);
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
}