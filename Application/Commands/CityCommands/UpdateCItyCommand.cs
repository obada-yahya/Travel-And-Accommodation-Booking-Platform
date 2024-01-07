using MediatR;

namespace Application.Commands.CityCommands;

public record UpdateCityCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string PostOffice { get; set; } = string.Empty;
}