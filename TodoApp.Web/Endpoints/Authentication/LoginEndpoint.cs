/*using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using TodoApp.Core.Entities;
using TodoApp.UseCases.DTOs;
using TodoApp.UseCases.Services;

namespace TodoApp.Web.Endpoints.Authentication;

// [HttpPost("/api/custom-login")]
public class CustomLoginEndpoint : Endpoint<LoginRequestDTO, LoginResponseDTO>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomLoginEndpoint> _logger;

    private readonly JwtService _jwtService;

    public CustomLoginEndpoint(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IConfiguration configuration,
        ILogger<CustomLoginEndpoint> logger,
        JwtService jwtService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _jwtService = jwtService;
    }

    public override void Configure()
    {
        Post("/api/custom-login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequestDTO req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);

        _logger.LogInformation($"User found: {user?.Email}");

        if (user == null)
        {
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);

        _logger.LogInformation($"Sign in result: {signInResult.Succeeded}");

        if (!signInResult.Succeeded)
        {
            await SendErrorsAsync(StatusCodes.Status401Unauthorized, ct);
            return;
        }

        // var token = _jwtService.GenerateJwtToken(user);

        var response = new LoginResponseDTO
        {
            UserId = user.Id,
            Email = user.Email,
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresIn = 3600, // Set to your desired expiration time
            RefreshToken = JwtService.GenerateRefreshToken().Token
        };

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}*/