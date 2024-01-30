using Application.Queries.ReviewQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.ReviewHandler;

public class CheckReviewExistenceForBookingQueryHandler : IRequestHandler<CheckReviewExistenceForBookingQuery,bool>
{
    private readonly IReviewRepository _reviewRepository;

    public CheckReviewExistenceForBookingQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<bool> Handle(CheckReviewExistenceForBookingQuery request, CancellationToken cancellationToken)
    {
        return await _reviewRepository.DoesBookingHaveReviewAsync(request.BookingId);
    }
}