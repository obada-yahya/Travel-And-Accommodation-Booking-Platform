using Application.DTOs.RoomDtos;
using FluentValidation;

namespace TAABP.API.Validators.HotelValidators;

public class GetHotelAvailableRoomsValidator : GenericValidator<GetHotelAvailableRoomsDto>
{
    public GetHotelAvailableRoomsValidator()
    {
        RuleFor(room => room.CheckInDate)
            .GreaterThan(DateTime.Today)
            .WithMessage("Check-in must be in the future.");

        RuleFor(booking => booking.CheckOutDate)
            .GreaterThanOrEqualTo(booking => booking.CheckInDate.AddDays(1))
            .WithMessage("Check-out must be at least one day after check-in.");
    }
}