using Application.DTOs.ReviewsDtos;
using Application.Queries.ReviewQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Handlers.ReviewHandlers;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginatedList<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var paginatedList = await 
            _reviewRepository
                .GetAllByHotelIdAsync(
                    request.HotelId,
                    request.SearchQuery,
                    request.PageNumber,
                    request.PageSize);

        return new PaginatedList<ReviewDto>(
            _mapper.Map<List<ReviewDto>>(paginatedList.Items),
            paginatedList.PageData);
    }
}