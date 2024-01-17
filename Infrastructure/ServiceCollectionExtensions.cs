using Domain.Common.Interfaces;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<ICityRepository, CityRepository>();
        return services;
    }
}