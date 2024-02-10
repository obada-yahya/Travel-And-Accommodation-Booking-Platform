using Application.DTOs.DiscountDtos;
using Application.Queries.DiscountQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Exceptions;
using MediatR;

namespace Application.Handlers.DiscountHandlers;

public class GetAllRoomTypeDiscountsQueryHandler : IRequestHandler<GetAllRoomTypeDiscountsQuery, PaginatedList<DiscountDto>>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public GetAllRoomTypeDiscountsQueryHandler(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DiscountDto>> Handle(GetAllRoomTypeDiscountsQuery request, CancellationToken cancellationToken)
    {
        var paginatedList = await 
            _discountRepository
                .GetAllByRoomTypeIdAsync(
                    request.RoomTypeId,
                    request.PageNumber,
                    request.PageSize);

        return new PaginatedList<DiscountDto>(
            _mapper.Map<List<DiscountDto>>(paginatedList.Items),
            paginatedList.PageData);
    }
}