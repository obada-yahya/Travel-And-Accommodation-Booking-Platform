using Application.DTOs.DiscountDtos;
using MediatR;

namespace Application.Commands.DiscountCommands;

public record CreateDiscountCommand : IRequest<DiscountDto?>
{
    public Guid RoomTypeId { get; set; }
    public float DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}