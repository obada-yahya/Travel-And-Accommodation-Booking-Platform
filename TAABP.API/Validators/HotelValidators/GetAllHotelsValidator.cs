using Application.Queries.HotelQueries;
using FluentValidation;

namespace TAABP.API.Validators.HotelValidators;

public class GetAllHotelsValidator : GenericValidator<GetAllHotelsQuery>
{
    public GetAllHotelsValidator()
    {
        RuleFor(roomAmenity => roomAmenity.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(roomAmenity => roomAmenity.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThan(21)
            .WithMessage("Page Size can't be greater than 20");
    }
}