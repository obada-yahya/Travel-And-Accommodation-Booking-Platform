using Application.DTOs.ReviewsDtos;
using FluentValidation;

namespace TAABP.API.Validators.ReviewsValidators;

public class CreateReviewValidator : GenericValidator<ReviewForCreationDto>
{
    public CreateReviewValidator()
    {
        RuleFor(review => review.BookingId)
            .NotEmpty().WithMessage("BookingId field shouldn't be empty");

        RuleFor(review => review.Comment)
            .NotEmpty()
            .WithMessage("Comment field shouldn't be empty");

        RuleFor(review => review.Rating)
            .InclusiveBetween(0, 5)
            .WithMessage("Rating must be between 0 and 5.");
    }
}