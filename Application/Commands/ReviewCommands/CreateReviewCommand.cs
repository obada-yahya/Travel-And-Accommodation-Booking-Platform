using Application.DTOs.ReviewsDtos;
using MediatR;

namespace Application.Commands.ReviewCommands;

public class CreateReviewCommand : IRequest<ReviewDto?>
{
    public Guid BookingId { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.Today;
    public float Rating { get; set; }
}