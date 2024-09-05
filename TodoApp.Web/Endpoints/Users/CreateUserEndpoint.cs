using FastEndpoints;
using TodoApp.Core.DTOs;
using TodoApp.UseCases.Services;

namespace TodoApp.Web.Endpoints.Users;

public class CreateUserEndpoint(UserService userService) : Endpoint<CreateUserDTO, CreateUserResponseDTO>
{
    private readonly UserService _userService = userService;

    public override void Configure()
    {
        Post("/api/user/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserDTO request, CancellationToken ct)
    {
        CreateUserResponseDTO NewUser = await _userService.AddUserAsync(request);

        if (NewUser == null)
        {
            AddError("User already exists");

            await SendErrorsAsync(409, ct); // Conflict
        }
        else
        {
            await SendAsync(NewUser, StatusCodes.Status201Created, ct);
        }
    }
}
