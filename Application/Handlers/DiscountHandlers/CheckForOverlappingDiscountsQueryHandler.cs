using Application.Queries.DiscountQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.DiscountHandlers;

public class CheckForOverlappingDiscountsQueryHandler : 
IRequestHandler<CheckForOverlappingDiscountQuery,bool>
{
    private readonly IDiscountRepository _discountRepository;

    public CheckForOverlappingDiscountsQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public Task<bool> Handle(CheckForOverlappingDiscountQuery request, CancellationToken cancellationToken)
    {
        return _discountRepository.HasOverlappingDiscountAsync(request.RoomTypeId,
                   request.FromDate,
                   request.ToDate);
    }
}