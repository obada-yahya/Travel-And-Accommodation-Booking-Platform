using Domain.Common.Interfaces;
using Infrastructure.Auth;
using Infrastructure.Auth.AuthUser;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using PasswordHashing;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IAppUserRepository, AppUserRepository>();
        services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IPasswordGenerator, Argon2PasswordGenerator>();
        services.AddTransient<IAuthUser, AuthUser>();
        return services;
    }
}