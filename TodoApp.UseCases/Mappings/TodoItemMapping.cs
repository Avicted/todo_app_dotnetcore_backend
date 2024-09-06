
using AutoMapper;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;

namespace TodoApp.UseCases.Mappings;
public class TodoItemMapping : Profile
{
    public TodoItemMapping()
    {
        CreateMap<TodoItem, UpdateTodoItemDTO>();
        CreateMap<TodoItem, CreateTodoItemResponseDTO>();
        CreateMap<TodoItem, CreateTodoItemDTO>();

        CreateMap<CreateTodoItemResponseDTO, TodoItem>();
        CreateMap<UpdateTodoItemDTO, TodoItem>();
        CreateMap<CreateTodoItemDTO, TodoItem>();

        CreateMap<UpdateTodoItemDTO, UpdateTodoItemResponseDTO>();
        CreateMap<UpdateTodoItemResponseDTO, UpdateTodoItemDTO>();

        // Delete
        CreateMap<TodoItem, DeleteTodoItemDTO>();
        CreateMap<DeleteTodoItemDTO, TodoItem>();
        CreateMap<DeleteTodoItemDTO, DeleteTodoItemResponseDTO>();
        CreateMap<DeleteTodoItemResponseDTO, DeleteTodoItemDTO>();
    }
}
