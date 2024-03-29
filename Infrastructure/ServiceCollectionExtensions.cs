﻿using Domain.Common.Interfaces;
using Infrastructure.Auth.AuthUser;
using Infrastructure.Auth.Token;
using Infrastructure.Common.Persistence.Repositories;
using Infrastructure.Email;
using Infrastructure.Email.Models;
using Infrastructure.ImageStorage;
using Infrastructure.Pdf;
using Microsoft.Extensions.DependencyInjection;
using PasswordHashing;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IPasswordGenerator, Argon2PasswordGenerator>();
        services.AddScoped<IAuthUser, AuthUser>();
        services.AddScoped<IImageService, FireBaseImageService>();
        services.AddScoped<IRoomAmenityRepository, RoomAmenityRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}