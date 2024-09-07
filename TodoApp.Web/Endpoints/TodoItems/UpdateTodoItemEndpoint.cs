using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Services;
using System.Security.Claims;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using TodoApp.Core.Enums;

namespace TodoApp.Web.Endpoints.TodoItems;

public class UpdateTodoItemEndpoint : Endpoint<UpdateTodoItemDTO, UpdateTodoItemResponseDTO>
{
    private readonly TodoItemService _todoItemService;
    private readonly ILogger<UpdateTodoItemEndpoint> _logger;

    private readonly UserManager<User> _userManager;

    // Inject the logger via the constructor
    public UpdateTodoItemEndpoint(TodoItemService todoItemService, ILogger<UpdateTodoItemEndpoint> logger, UserManager<User> userManager)
    {
        _todoItemService = todoItemService;
        _logger = logger;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Put("/api/todos/{@id}", r => new { r.Id });

        // AllowAnonymous();

        // Document in Swagger that this endpoint returns only the UserName and id
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Update a new TodoItem";
            s.Description = "The endpoint Updates a new TodoItem for the authenticated user.";
            s.ExampleRequest = new TodoItem { Title = "example title", Description = "example description", Status = TodoItemStatus.NotStarted, UserId = "example" };
            s.ResponseExamples[200] = new UpdateTodoItemResponseDTO { Id = 1, Title = "example title", Description = "example description", Status = TodoItemStatus.NotStarted, UserId = "example" };
        });
    }

    public override async Task HandleAsync(UpdateTodoItemDTO request, CancellationToken ct)
    {
        // Get the user's email from the claims
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        // If the email is not found, return a 401
        if (email == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        // Get the user from the email
        var user = await _userManager.FindByEmailAsync(email);
        var userId = await _userManager.GetUserIdAsync(user);

        _logger.LogInformation("User found: {0}", user?.Id);

        // If the user is not found, return a 404
        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        // Get the TodoItem by the ID
        var todoItemDTO = new GetTodoItemByIdDTO { Id = request.Id };
        var todoItem = await _todoItemService.GetTodoItemByIdAsync(user.Id, todoItemDTO.Id);

        _logger.LogInformation("TodoItem found: {0}", todoItem?.Id);

        if (todoItem == null)
        {
            AddError("TodoItem not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        // Update the TodoItem
        var updatedTodoItem = new UpdateTodoItemDTO
        {
            Id = todoItem.Id,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status
        };

        // Update the TodoItem
        UpdateTodoItemResponseDTO updatedTodoItemRes = await _todoItemService.UpdateTodoItemAsync(userId, updatedTodoItem);

        // Log the TodoItem creation
        _logger.LogInformation("TodoItem Updated: {0}", updatedTodoItemRes.Id);

        if (updatedTodoItemRes == null)
        {
            AddError("TodoItem not updated");
            await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // Return the TodoItem
        await SendAsync(updatedTodoItemRes, StatusCodes.Status200OK, ct);
    }
}
