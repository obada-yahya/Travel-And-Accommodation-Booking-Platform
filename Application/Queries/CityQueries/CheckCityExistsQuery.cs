using MediatR;

namespace Application.Queries.CityQueries;

public class CheckCityExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
}