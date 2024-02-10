using Application.Queries.DiscountQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.DiscountHandlers;

public class CheckDiscountExistsQueryHandler : IRequestHandler<CheckDiscountExistsQuery, bool>
{
    private readonly IDiscountRepository _discountRepository;

    public CheckDiscountExistsQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<bool> Handle(CheckDiscountExistsQuery request, CancellationToken cancellationToken)
    {
        return await _discountRepository.IsExistsAsync(request.Id);
    }
}