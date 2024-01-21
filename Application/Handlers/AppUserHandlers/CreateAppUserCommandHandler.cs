using Application.Commands.AppUserCommands;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;
using PasswordHashing;

namespace Application.Handlers.AppUserHandlers;

public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand>
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordGenerator _passwordGenerator;
    public CreateAppUserCommandHandler(IAppUserRepository appUserRepository, IMapper mapper, IPasswordGenerator passwordGenerator)
    {
        _appUserRepository = appUserRepository;
        _mapper = mapper;
        _passwordGenerator = passwordGenerator;
    }

    public async Task Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
    {
        var appUser = _mapper.Map<AppUser>(request);
        var uniqueUserSalt = _passwordGenerator.GenerateSalt();
        var userPasswordHash = _passwordGenerator.GenerateHashedPassword(request.Password, uniqueUserSalt);
        if (userPasswordHash is null) throw new InvalidOperationException("Can't Hash User Password");
        
        appUser.Id = Guid.NewGuid();
        appUser.PasswordHash = userPasswordHash;
        appUser.Salt = Convert.ToBase64String(uniqueUserSalt);
        appUser.Role = "User";
        await _appUserRepository.InsertAsync(appUser);
    }
}