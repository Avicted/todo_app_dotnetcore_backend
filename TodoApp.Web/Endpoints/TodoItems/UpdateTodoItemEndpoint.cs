using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Services;
using System.Security.Claims;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        Put("/api/todo/{@id}", r => new { r.Id });

        // AllowAnonymous();

        // Document in Swagger that this endpoint returns only the UserName and id
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Update a new TodoItem";
            s.Description = "The endpoint Updates a new TodoItem for the authenticated user.";
            s.ExampleRequest = new TodoItem { Title = "example title", Description = "example description", IsCompleted = false };
            s.ResponseExamples[200] = new UpdateTodoItemResponseDTO { Id = 1, Title = "example title", Description = "example description", IsCompleted = false, UserId = "example" };
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

        _logger.LogInformation("User found: {0}", user?.Id);

        // If the user is not found, return a 404
        if (user == null)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        // Get the TodoItem by the ID
        var todoItem = await _todoItemService.GetTodoItemByIdAsync(request.Id);

        _logger.LogInformation("TodoItem found: {0}", todoItem?.Id);

        if (todoItem == null)
        {
            AddError("TodoItem not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        // Update a the TodoItem
        var updatedTodoItem = new UpdateTodoItemDTO
        {
            Id = todoItem.Id,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsCompleted,
        };

        // Update the TodoItem
        UpdateTodoItemResponseDTO updatedTodoItemRes = await _todoItemService.UpdateTodoItemAsync(updatedTodoItem);

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
