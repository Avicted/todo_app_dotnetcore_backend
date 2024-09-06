#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;

namespace TodoApp.UseCases.Interfaces;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync(string userId);
    Task<GetTodoItemByIdResponseDTO?> GetByIdAsync(string userId, int todoItemId);
    Task<CreateTodoItemResponseDTO> AddAsync(CreateTodoItemDTO item);
    Task<UpdateTodoItemResponseDTO?> UpdateAsync(UpdateTodoItemDTO item);
    Task<DeleteTodoItemResponseDTO?> DeleteAsync(DeleteTodoItemDTO item);
}