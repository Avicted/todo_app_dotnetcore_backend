using Microsoft.Extensions.DependencyInjection;
using TodoApp.UseCases.Interfaces;
using TodoApp.UseCases.Services;

namespace TodoApp.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddScoped<TodoItemService>(); // Register TodoItemService

        return services;
    }
}
