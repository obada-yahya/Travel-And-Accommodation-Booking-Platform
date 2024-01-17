using MediatR;

namespace Application.Commands.CityCommands;

public record DeleteCityCommand : IRequest
{
    public Guid Id { get; set; }
}