using Application.Commands.UserCommands;
using Application.DTOs.AppUserDtos;
using AutoMapper;
using Infrastructure.Auth.Models;
using Infrastructure.Auth.Token;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.AuthValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AuthenticationController(IConfiguration configuration, ITokenGenerator tokenGenerator, IMapper mapper, IMediator mediator)
    {
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
        _mapper = mapper;
        _mediator = mediator;
    }
    
    /// <summary>
    /// Endpoint for user sign-in. Validates user credentials and generates a JWT token upon successful authentication.
    /// </summary>
    /// <param name="email">The email address of the user attempting to sign in.</param>
    /// <param name="password">The password associated with the user's account.</param>
    /// <returns>
    /// If successful, returns the generated JWT token; otherwise, returns a list of validation errors or unauthorized status.
    /// </returns>
    [HttpPost("SignIn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> SignIn(
        AuthenticationRequestBody authenticationRequestBody)
    {
        var validator = new AuthenticationRequestBodyValidator();
        var errors = await validator.CheckForValidationErrorsAsync(authenticationRequestBody);
        if (errors.Count > 0) return BadRequest(errors);
        
        var user = await _tokenGenerator.ValidateUserCredentials(
            authenticationRequestBody.Email,
            authenticationRequestBody.Password);
        if (user is null) return Unauthorized();
        
        var secretKey = _configuration["Authentication:SecretForKey"];
        var issuer = _configuration["Authentication:Issuer"];
        var audience = _configuration["Authentication:Audience"];
        var token = await _tokenGenerator
                               .GenerateToken(
                                   user.Email,
                                   authenticationRequestBody.Password,
                                   secretKey,
                                   issuer,
                                   audience);
        return Ok(token);
    }

    /// <summary>
    /// Registers a new user with the provided credentials.
    /// </summary>
    /// <param name="appUserForCreationDto">User registration details.</param>
    /// <returns>An action result indicating success or failure of the registration process.</returns>
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Register(
        UserForCreationDto appUserForCreationDto)
    {
        try
        {
            var validator = new RegisterRequestBodyValidator();
            var errors = await validator.CheckForValidationErrorsAsync(appUserForCreationDto);
            if (errors.Count > 0) return BadRequest(errors);
            
            var request = _mapper.Map<CreateUserCommand>(appUserForCreationDto);
            await _mediator.Send(request);
            return Ok("Register User Successfully.");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}
