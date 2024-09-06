
using AutoMapper;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;

namespace TodoApp.UseCases.Mappings;
public class TodoItemMapping : Profile
{
    public TodoItemMapping()
    {
        // Create
        CreateMap<TodoItem, CreateTodoItemDTO>();
        CreateMap<CreateTodoItemDTO, TodoItem>();
        CreateMap<CreateTodoItemDTO, CreateTodoItemResponseDTO>();
        CreateMap<CreateTodoItemResponseDTO, CreateTodoItemDTO>();

        // Read
        CreateMap<TodoItem, GetTodoItemByIdDTO>();
        CreateMap<TodoItem, GetTodoItemByIdResponseDTO>(); // Add this mapping

        CreateMap<GetTodoItemByIdDTO, TodoItem>();
        CreateMap<GetTodoItemByIdDTO, GetTodoItemByIdResponseDTO>();
        CreateMap<GetTodoItemByIdResponseDTO, GetTodoItemByIdDTO>();

        // Update
        CreateMap<TodoItem, UpdateTodoItemDTO>();
        CreateMap<UpdateTodoItemDTO, TodoItem>();
        CreateMap<UpdateTodoItemDTO, UpdateTodoItemResponseDTO>();
        CreateMap<UpdateTodoItemResponseDTO, UpdateTodoItemDTO>();

        // Delete
        CreateMap<TodoItem, DeleteTodoItemDTO>();
        CreateMap<DeleteTodoItemDTO, TodoItem>();
        CreateMap<DeleteTodoItemDTO, DeleteTodoItemResponseDTO>();
        CreateMap<DeleteTodoItemResponseDTO, DeleteTodoItemDTO>();
    }
}
