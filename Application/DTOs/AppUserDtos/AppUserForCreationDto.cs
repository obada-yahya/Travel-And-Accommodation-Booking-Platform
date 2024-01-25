﻿namespace Application.DTOs.AppUserDtos;

public record AppUserForCreationDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}