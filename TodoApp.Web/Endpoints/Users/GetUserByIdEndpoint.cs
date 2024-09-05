using FastEndpoints;
using TodoApp.UseCases.DTOs;
using TodoApp.Core.Entities;
using TodoApp.UseCases.Services;

namespace TodoApp.Web.Endpoints.Users;

public class GetUserByIdEndpoint(UserService userService) : Endpoint<GetUserByIdDTO, GetUserByIdResponseDTO>
{
    private readonly UserService _userService = userService;

    public override void Configure()
    {
        Get("/api/user/{id}");

        // @Todo(Avic): Add authentication, when login is implemented
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserByIdDTO request, CancellationToken ct)
    {
        User user = await _userService.GetUserByIdAsync(request.Id);

        if (user == null)
        {
            AddError("User not found");

            await SendErrorsAsync(StatusCodes.Status404NotFound, ct); // Not Found
        }
        else
        {
            await SendAsync(new GetUserByIdResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }, StatusCodes.Status200OK, ct); // OK
        }
    }
}
