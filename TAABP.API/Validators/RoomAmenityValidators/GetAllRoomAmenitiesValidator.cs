using Application.Queries.RoomAmenityQueries;
using FluentValidation;
using Infrastructure.Auth.Models;

namespace TAABP.API.Validators.RoomAmenityValidators;

public class GetAllRoomAmenitiesValidator : GenericValidator<GetAllRoomAmenitiesQuery>
{
    public GetAllRoomAmenitiesValidator()
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
    
    public async Task<List<ErrorModel>> CheckForValidationErrorsAsync(GetAllRoomAmenitiesQuery request)
    {
        var results = Validate(request);
    
        return !results.IsValid
            ? await Task.FromResult(results.Errors.Select(failure =>
                new ErrorModel
                {
                    FieldName = failure.PropertyName,
                    Message = failure.ErrorMessage
                }).ToList())
            : new List<ErrorModel>();
    }
}