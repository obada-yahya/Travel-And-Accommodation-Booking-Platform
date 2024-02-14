using Application.Queries.CityQueries;
using FluentValidation;

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
            .WithMessage("Page size must be greater than 0.");
    }
}