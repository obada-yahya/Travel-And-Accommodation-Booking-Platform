using FluentValidation;
using Infrastructure.Auth.Models;

namespace TAABP.API.Validators;

public class GenericValidator<T> : AbstractValidator<T>
{
    public async Task<List<ErrorModel>> CheckForValidationErrorsAsync(T request)
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