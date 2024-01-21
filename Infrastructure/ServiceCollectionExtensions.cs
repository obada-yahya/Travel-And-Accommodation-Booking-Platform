using Domain.Common.Interfaces;
using Infrastructure.Auth.AuthUser;
using Infrastructure.Auth.Token;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using PasswordHashing;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IPasswordGenerator, Argon2PasswordGenerator>();
        services.AddScoped<IAuthUser, AuthUser>();
        return services;
    }
}