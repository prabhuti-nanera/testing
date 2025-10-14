using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;

namespace CRC.WebPortal.API.Controllers;

/// - UI sends simple data structures (DTOs) 
/// - API internally creates and executes appropriate CQRS commands
/// - UI doesn't know about internal command structure (encapsulation)
/// - One API action may trigger multiple commands internally
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> SignUp([FromBody] SignupDataRequest request)
    {
        // API creates the command internally - UI doesn't need to know about commands
        var command = new SignUpCommand
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("signin")]
    public async Task<ActionResult<AuthResponse>> SignIn([FromBody] SigninDataRequest request)
    {
        // API creates the command internally - UI doesn't need to know about commands
        var command = new SignInCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

}

/// Simple data structure for SignUp API - UI sends only required data
/// API internally creates SignUpCommand and may execute multiple operations

public class SignupDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}


/// Simple data structure for SignIn API - UI sends only required data
/// API internally creates SignInCommand and may execute multiple operations
public class SigninDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}
