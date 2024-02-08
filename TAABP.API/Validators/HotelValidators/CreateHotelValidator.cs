using Application.DTOs.HotelDtos;
using FluentValidation;

namespace TAABP.API.Validators.HotelValidators;

public class CreateHotelValidator : GenericValidator<HotelForCreationDto>
{
    public CreateHotelValidator()
    {
        RuleFor(hotel => hotel.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(hotel => hotel.Rating)
            .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");

        RuleFor(hotel => hotel.StreetAddress)
            .NotEmpty().WithMessage("Street address is required.")
            .MaximumLength(200).WithMessage("Street address cannot exceed 200 characters.");

        RuleFor(hotel => hotel.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(hotel => hotel.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters.");
        
        RuleFor(hotel => hotel.FloorsNumber)
            .GreaterThan(0).WithMessage("Number of floors must be greater than 0.");
    }
}