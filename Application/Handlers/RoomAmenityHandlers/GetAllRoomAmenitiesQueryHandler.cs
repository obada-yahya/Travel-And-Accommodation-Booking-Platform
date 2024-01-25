using Application.DTOs.RoomAmenityDtos;
using Application.Queries.RoomAmenityQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Handlers.RoomAmenityHandlers;

public class GetAllRoomAmenitiesQueryHandler : IRequestHandler<GetAllRoomAmenitiesQuery, PaginatedList<RoomAmenityDto>>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IMapper _mapper;

    public GetAllRoomAmenitiesQueryHandler(IRoomAmenityRepository roomAmenityRepository, IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoomAmenityDto>> Handle(GetAllRoomAmenitiesQuery request, CancellationToken cancellationToken)
    {
        var paginatedList = await 
            _roomAmenityRepository
                .GetAllAsync(
                    request.SearchQuery,
                    request.PageNumber,
                    request.PageSize);

        return new PaginatedList<RoomAmenityDto>(
            _mapper.Map<List<RoomAmenityDto>>(paginatedList.Items),
            paginatedList.PageData);
    }
}