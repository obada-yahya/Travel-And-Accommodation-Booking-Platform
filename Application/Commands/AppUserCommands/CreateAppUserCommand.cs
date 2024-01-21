namespace Application.Commands.AppUserCommands;
using MediatR;

public record CreateAppUserCommand : IRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
