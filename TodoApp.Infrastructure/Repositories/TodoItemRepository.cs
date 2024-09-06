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

    public async Task DeleteAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item != null)
        {
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
