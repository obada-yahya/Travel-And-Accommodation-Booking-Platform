using MediatR;

namespace Application.Commands.DiscountCommands;

public record DeleteDiscountCommand : IRequest
{
    public Guid Id { get; set; }
}