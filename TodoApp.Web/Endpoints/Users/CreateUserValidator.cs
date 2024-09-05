using FastEndpoints;
using FluentValidation;
using TodoApp.Core.DTOs;

namespace TodoApp.Web.Endpoints.Users;

public class CreateUserValidator : Validator<CreateUserDTO>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}