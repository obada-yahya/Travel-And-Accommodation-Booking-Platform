using Application.DTOs.DiscountDtos;
using Application.Queries.DiscountQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.DiscountHandlers;

public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery,DiscountDto>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public GetDiscountByIdQueryHandler(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<DiscountDto> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id);
        return _mapper.Map<DiscountDto>(discount);
    }
}