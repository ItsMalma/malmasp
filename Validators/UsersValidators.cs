using FluentValidation;
using Malmasp.Dtos;

namespace Malmasp.Validators;

public class UserRequestDtoValidator : AbstractValidator<UserRequestDto>
{
    public UserRequestDtoValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("Must be filled")
            .MaximumLength(128)
            .WithMessage("Maximum 128 characters");
        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Must be filled")
            .MinimumLength(8)
            .WithMessage("Minimum 8 characters");
    }
}