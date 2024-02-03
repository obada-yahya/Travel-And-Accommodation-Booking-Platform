using Application.DTOs.RoomDtos;
using Application.Queries.RoomQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Handlers.RoomHandlers;

public class GetRoomsByHotelIdQueryHandler : IRequestHandler<GetRoomsByHotelIdQuery, PaginatedList<RoomDto>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsByHotelIdQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoomDto>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var paginatedList = await 
            _roomRepository.GetRoomsByHotelIdAsync
            (request.HotelId,
            request.SearchQuery, 
            request.PageNumber,
            request.PageSize);

        return new PaginatedList<RoomDto>(
            _mapper.Map<List<RoomDto>>(paginatedList.Items),
            paginatedList.PageData);
    }
}