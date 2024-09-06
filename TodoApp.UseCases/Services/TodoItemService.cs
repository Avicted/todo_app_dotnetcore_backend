using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.UseCases.Interfaces;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;
using AutoMapper;

namespace TodoApp.UseCases.Services;

public class TodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IMapper _mapper;

    public TodoItemService(ITodoItemRepository todoItemRepository, IMapper mapper)
    {
        _todoItemRepository = todoItemRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
    {
        return await _todoItemRepository.GetAllAsync();
    }

    public async Task<TodoItem> GetTodoItemByIdAsync(int id)
    {
        return await _todoItemRepository.GetByIdAsync(id);
    }

    public async Task<CreateTodoItemResponseDTO> AddTodoItemAsync(CreateTodoItemDTO todoItem)
    {
        return await _todoItemRepository.AddAsync(todoItem);
    }

    public async Task<UpdateTodoItemResponseDTO> UpdateTodoItemAsync(UpdateTodoItemDTO todoItem)
    {
        return _mapper.Map<UpdateTodoItemResponseDTO>(await _todoItemRepository.UpdateAsync(todoItem));
    }

    public async Task<DeleteTodoItemResponseDTO> DeleteTodoItemAsync(DeleteTodoItemDTO todoItem)
    {
        return _mapper.Map<DeleteTodoItemResponseDTO>(await _todoItemRepository.DeleteAsync(todoItem));
    }

}