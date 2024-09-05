using Microsoft.EntityFrameworkCore;
using TodoApp.UseCases.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using TodoApp.UseCases.Services; // Namespace for your repository implementation
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.Cookies;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.
    AddFastEndpoints().
    SwaggerDocument(); //define a swagger document for the API

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers(); // Adds services for controllers (API)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository implementation
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>(); // Register ITodoItemRepository to its implementation
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Register IUserRepository to its implementation

// Register application services
builder.Services.AddScoped<TodoItemService>(); // Register TodoItemService
builder.Services.AddScoped<UserService>(); // Register UserService

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adds Swagger generation services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.UseRouting();
app.UseAuthorization();
// app.MapControllers(); // Maps attribute-routed controllers

app.Run();