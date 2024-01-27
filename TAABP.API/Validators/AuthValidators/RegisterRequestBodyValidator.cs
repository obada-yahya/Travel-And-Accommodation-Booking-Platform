using Application.DTOs.AppUserDtos;
using FluentValidation;

namespace TAABP.API.Validators.AuthValidators;

public class RegisterRequestBodyValidator : GenericValidator<UserForCreationDto>
{
    public RegisterRequestBodyValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.FirstName)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(password => password.Any(char.IsUpper) && password.Any(char.IsLower))
            .WithMessage("Password must contain both uppercase and lowercase characters.")
            .Must(password => password.Any(char.IsDigit))
            .WithMessage("Password must contain at least one digit.")
            .Must(password => password.Any(ch => !char.IsLetterOrDigit(ch)))
            .WithMessage("Password must contain at least one special character.");
    }
}