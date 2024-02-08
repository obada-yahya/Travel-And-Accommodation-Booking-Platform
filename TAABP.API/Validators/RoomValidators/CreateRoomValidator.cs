using Application.DTOs.RoomDtos;
using FluentValidation;

namespace TAABP.API.Validators.RoomValidators;

public class CreateRoomValidator : GenericValidator<RoomForCreationDto>
{
    public CreateRoomValidator()
    {
        RuleFor(room => room.RoomTypeId)
            .NotEmpty().WithMessage("BookingId field shouldn't be empty");

        RuleFor(room => room.Rating)
            .InclusiveBetween(0, 5)
            .WithMessage("Rating must be between 0 and 5.");
        
        RuleFor(room => room.View)
            .NotEmpty().WithMessage("View is required.")
            .MaximumLength(200).WithMessage("View description cannot exceed 200 characters.");
        
        RuleFor(room => room.AdultsCapacity)
            .InclusiveBetween(1,4)
            .WithMessage("Number of adults must be between 1 and 4.");
        
        RuleFor(room => room.ChildrenCapacity)
            .InclusiveBetween(0,2)
            .WithMessage("Number of children must be between 0 and 2.");
    }
}