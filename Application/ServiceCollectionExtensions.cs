using Application.Profiles;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(typeof(CityProfile));
        services.AddAutoMapper(typeof(HotelProfile));
        services.AddAutoMapper(typeof(RoomAmenityProfile));
        services.AddAutoMapper(typeof(ReviewProfile));
        services.AddAutoMapper(typeof(RoomProfile));
        
        return services;
    }
}