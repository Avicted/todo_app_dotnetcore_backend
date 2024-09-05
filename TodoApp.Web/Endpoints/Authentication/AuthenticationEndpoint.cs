using FastEndpoints;
using TodoApp.Infrastructure.Services;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Services;

namespace TodoApp.Web.Endpoints.Authentication;

public class AuthEndpoint : Endpoint<LoginDTO>
{
    private readonly JwtService _jwtService;
    private readonly UserService _userService;

    public AuthEndpoint(JwtService jwtService, UserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Authenticate user and return a JWT token";
            s.Description = "This endpoint authenticates a user and returns a JWT token if the credentials are valid.";
            s.Responses[200] = "Token returned";
            s.Responses[401] = "Unauthorized";
        });
    }

    public override async Task HandleAsync(LoginDTO request, CancellationToken ct)
    {
        // Validate user credentials
        var user = await _userService.GetUserByEmailAndPasswordAsync(request.Email, request.Password);

        if (user == null)
        {
            // Invalid credentials
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        // Return the token
        await SendAsync(new { Token = token }, StatusCodes.Status200OK, ct);
    }
}