using Application.DTOs.BookingDtos;
using FluentValidation;

namespace TAABP.API.Validators.BookingValidators;

public class ReserveRoomValidator : GenericValidator<ReserveRoomDto>
{
    public ReserveRoomValidator()
    {
        RuleFor(booking => booking.CheckInDate)
            .GreaterThan(DateTime.Today)
            .WithMessage("Can't book a room in the same day or in the past for check-in");
        
        RuleFor(booking => booking.CheckOutDate)
            .GreaterThanOrEqualTo(booking => booking.CheckInDate.AddDays(1))
            .WithMessage("Check-out date must be at least one day after the check-in date");
    }
}