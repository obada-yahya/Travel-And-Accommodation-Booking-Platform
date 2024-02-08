using System.Diagnostics;
using MediatR;

namespace Application.Queries.HotelQueries;

public record CheckHotelExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
}