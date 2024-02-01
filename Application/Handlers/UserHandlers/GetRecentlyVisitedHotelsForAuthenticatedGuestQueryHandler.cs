using Application.DTOs.HotelDtos;
using Application.Queries.UserQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.UserHandlers;

public class GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandler :
    IRequestHandler<GetRecentlyVisitedHotelsForAuthenticatedGuestQuery, List<HotelWithoutRoomsDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<HotelWithoutRoomsDto>> Handle(GetRecentlyVisitedHotelsForAuthenticatedGuestQuery request,
        CancellationToken cancellationToken)
    {
        return _mapper.Map<List<HotelWithoutRoomsDto>>
        (await _userRepository
        .GetRecentlyVisitedHotelsForAuthenticatedGuestAsync
        (request.Email, request.Count));
    }
}