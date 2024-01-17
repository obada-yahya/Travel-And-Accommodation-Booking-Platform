using Application.Commands.CityCommands;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Handlers.CityHandlers;

public class DeleteCityHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public DeleteCityHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        if (!await _cityRepository.IsExistsAsync(request.Id))
        {
            throw new NotFoundException("City Doesn't Exists To Delete");
        }
        await _cityRepository.DeleteAsync(request.Id);
    }
}