using Application.Queries.CityQueries;
using FluentValidation;
using Infrastructure.Auth.Models;

namespace TAABP.API.Validators.CityValidators;

public class CitiesQueryValidator : GenericValidator<GetCitiesQuery>
{
    public CitiesQueryValidator()
    {
        RuleFor(city => city.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(city => city.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThan(21)
            .WithMessage("Page Size can't be greater than 20");
    }
}