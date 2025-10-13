using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;
using CRC.WebPortal.Application.Common.Features.Auth.Commands.ForgotPassword;
using CRC.WebPortal.Application.Common.Features.Auth.Commands.ResetPassword;

namespace CRC.WebPortal.API.Controllers;

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

    [HttpPost("forgot-password")]
    public async Task<ActionResult<AuthResponse>> ForgotPassword([FromBody] ForgotPasswordDataRequest request)
    {
        // API receives simple DTO and creates command internally (CQRS pattern)
        var command = new ForgotPasswordCommand
        {
            Email = request.Email
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<AuthResponse>> ResetPassword([FromBody] ResetPasswordDataRequest request)
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