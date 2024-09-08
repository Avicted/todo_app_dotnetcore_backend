using TodoApp.UseCases.Interfaces;
using TodoApp.Infrastructure.Persistense;
using TodoApp.Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using TodoApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using TodoApp.UseCases;
using TodoApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var AllowSpecificOrigins = "_AllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.
  AddFastEndpoints().
  SwaggerDocument(); //define a swagger document for the API

// Add services to the container.
builder.Services
    .AddAuthorization()
    .AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDataProtection();

builder.Services.AddAuthentication(options =>
{
    //  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
    options.UseSecurityTokenValidators = true;
});


builder.Services.AddControllers(); // Adds services for controllers (API)

// Dependency injection for infrastructure, Register infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Register repository implementation
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Dependency injection for use cases
builder.Services.AddUseCases(); // Register use cases

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adds Swagger generation services for FastEndpoints

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();

app.UseRouting();

app.UseCors(AllowSpecificOrigins);

app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();  // Ensure authorization is used

app.MapGroup("/api")
    .WithTags("Security")
    .MapIdentityApi<User>();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
