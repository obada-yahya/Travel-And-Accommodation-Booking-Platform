using Application.DTOs.CityDtos;
using MediatR;

namespace Application.Commands.CityCommands;

public record CreateCityCommand : IRequest<CityWithoutHotelsDto?>
{
    public string Name { get; set; }
    public string CountryName { get; set; }
    public string CountryCode { get; set; }
    public string PostOffice { get; set; }
}