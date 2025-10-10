using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;

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

    /// <summary>
    /// Health check endpoint for API
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { 
            status = "API is running", 
            timestamp = DateTime.UtcNow,
            message = "CRC WebPortal API - Clean Architecture Implementation"
        });
    }

    /// <summary>
    /// Receives signup data from UI and creates SignUpCommand internally
    /// This follows Clean Architecture - UI sends data, API creates commands
    /// </summary>
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

    /// <summary>
    /// Receives signin data from UI and creates SignInCommand internally
    /// This follows Clean Architecture - UI sends data, API creates commands
    /// </summary>
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

/// <summary>
/// Simple data transfer object for API - matches UI SignupData structure
/// </summary>
public class SignupDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

/// <summary>
/// Simple data transfer object for API - matches UI SigninData structure
/// </summary>
public class SigninDataRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}