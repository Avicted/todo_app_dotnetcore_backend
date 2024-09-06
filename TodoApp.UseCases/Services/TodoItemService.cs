using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.UseCases.Interfaces;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;

namespace TodoApp.UseCases.Services;

public class TodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemService(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
    {
        return await _todoItemRepository.GetAllAsync();
    }

    public async Task<TodoItem> GetTodoItemByIdAsync(int id)
    {
        return await _todoItemRepository.GetByIdAsync(id);
    }

    public async Task<CreateTodoItemResponseDTO> AddTodoItemAsync(TodoItem todoItem)
    {
        return await _todoItemRepository.AddAsync(todoItem);
    }

    public async Task UpdateTodoItemAsync(TodoItem todoItem)
    {
        await _todoItemRepository.UpdateAsync(todoItem);
    }

    public async Task DeleteTodoItemAsync(int id)
    {
        await _todoItemRepository.DeleteAsync(id);
    }
}