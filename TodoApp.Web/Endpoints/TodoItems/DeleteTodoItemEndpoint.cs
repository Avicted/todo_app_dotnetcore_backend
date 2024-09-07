using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Interfaces;

namespace TodoApp.Web.Endpoints.TodoItems;

public class DeleteTodoItemEndpoint : Endpoint<DeleteTodoItemDTO, DeleteTodoItemResponseDTO>
{
    private readonly ITodoItemRepository _repository;
    private readonly ILogger<DeleteTodoItemEndpoint> _logger;
    private readonly UserManager<User> _userManager;

    public DeleteTodoItemEndpoint(ITodoItemRepository repository, ILogger<DeleteTodoItemEndpoint> logger, UserManager<User> userManager)
    {
        _repository = repository;
        _logger = logger;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Delete("api/todos/{Id}");
        // AllowAnonymous();

        // Document in Swagger that this endpoint returns only the UserName and id
        Description(b => b.Produces(403));
        Summary(s =>
        {
            s.Summary = "Delete a TodoItem";
            s.Description = "The endpoint deletes a TodoItem for the authenticated user.";
            s.ExampleRequest = new DeleteTodoItemDTO { Id = 1 };
            s.ResponseExamples[200] = new DeleteTodoItemResponseDTO { Id = 1 };
        });
    }

    public override async Task<DeleteTodoItemResponseDTO> HandleAsync(DeleteTodoItemDTO request, CancellationToken cancellationToken)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _logger.LogInformation("Retrieving all todo items");
        _logger.LogInformation($"User userEmail: {userEmail}");


        var user = await _userManager.FindByEmailAsync(userEmail);
        var userId = await _userManager.GetUserIdAsync(user);

        if (userId == null)
        {
            AddError("Unauthorized");
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, cancellationToken);
            return null!;
        }

        var response = await _repository.DeleteAsync(userId, request);

        if (response == null)
        {
            _logger.LogInformation("TodoItem not found");

            AddError("TodoItem not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, cancellationToken);
        }

        _logger.LogInformation("TodoItem deleted: {0}", response?.Id);

        return response!;
    }
}
