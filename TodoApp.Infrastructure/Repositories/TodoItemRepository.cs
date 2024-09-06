using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Persistense;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Infrastructure.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TodoItemRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<CreateTodoItemResponseDTO> AddAsync(CreateTodoItemDTO item)
    {
        var todoItem = _mapper.Map<TodoItem>(item);

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateTodoItemResponseDTO>(todoItem);
    }

    public async Task<UpdateTodoItemResponseDTO?> UpdateAsync(UpdateTodoItemDTO item)
    {
        var todoItem = await _context.TodoItems.FindAsync(item.Id);
        if (todoItem == null)
        {
            return null;
        }

        // Update the properties of the TodoItem entity
        todoItem.Title = item.Title;
        todoItem.Description = item.Description;
        todoItem.IsCompleted = item.IsCompleted;

        // Save the changes to the database
        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateTodoItemResponseDTO>(item);
    }

    public async Task<DeleteTodoItemResponseDTO?> DeleteAsync(DeleteTodoItemDTO item)
    {
        var todoItem = await _context.TodoItems.FindAsync(item.Id);
        if (todoItem == null)
        {
            return null;
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return new DeleteTodoItemResponseDTO
        {
            Id = todoItem.Id,
        };
    }
}
