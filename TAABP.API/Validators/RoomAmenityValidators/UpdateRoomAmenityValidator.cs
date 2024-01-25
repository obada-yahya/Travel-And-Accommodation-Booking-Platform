using Application.DTOs.RoomAmenityDtos;
using FluentValidation;

namespace TAABP.API.Validators.RoomAmenityValidators;

public class UpdateRoomAmenityValidator : GenericValidator<RoomAmenityForUpdateDto>
{
    public UpdateRoomAmenityValidator()
    {
        RuleFor(roomAmenity => roomAmenity.Name)
            .NotEmpty()
            .WithMessage("Name field shouldn't be empty");

        RuleFor(roomAmenity => roomAmenity.Description)
            .NotEmpty()
            .WithMessage("Description field shouldn't be empty");
    }
}