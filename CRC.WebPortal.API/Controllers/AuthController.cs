using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CRC.Common.Models;
using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Shared.Requests;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;
using CRC.WebPortal.Application.Common.Features.Auth.Commands.ForgotPassword;
using CRC.WebPortal.Application.Common.Features.Auth.Commands.ResetPassword;

namespace CRC.WebPortal.API.Controllers;

/// - UI sends simple data structures (DTOs) 
/// - API internally creates and executes appropriate CQRS commands
/// - UI doesn't know about internal command structure (encapsulation)
/// - One API action may trigger multiple commands internally
[ApiController]
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IMapper _mapper;

    public AuthController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(AuthResponse), 400)]
    public async Task<IActionResult> SignUp([FromBody] SignupRequest request)
    {
        var command = _mapper.Map<SignUpCommand>(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(AuthResponse), 400)]
    public async Task<IActionResult> SignIn([FromBody] SigninRequest request)
    {
        var command = _mapper.Map<SignInCommand>(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

}

/// Simple data structure for SignUp API - UI sends only required data
/// API internally creates SignUpCommand and may execute multiple operations
    {
        // API receives simple DTO and creates command internally (CQRS pattern)
        var command = new ResetPasswordCommand
        {
            Email = request.Email,
            ResetCode = request.ResetCode,
            NewPassword = request.NewPassword
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
    {
        // API receives simple DTO and creates command internally (CQRS pattern)
        var command = new ResetPasswordCommand
        {
            Email = request.Email,
            ResetCode = request.ResetCode,
            NewPassword = request.NewPassword
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
    {
        // API receives simple DTO and creates command internally (CQRS pattern)
        var command = new ResetPasswordCommand
        {
            Email = request.Email,
            ResetCode = request.ResetCode,
            NewPassword = request.NewPassword
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

public class SignupDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class SigninDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}

public class ForgotPasswordDataRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string ResetCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}