using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;
using CRC.WebPortal.Application.Features.Auth.Commands.RefreshToken;
using CRC.WebPortal.Application.Features.Auth.Commands.SignOut;
using CRC.WebPortal.Application.Features.Auth.Queries.GetCurrentUser;

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
    public async Task<ActionResult<AuthResponse>> SignUp([FromBody] SignUpCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Ok(result);
    }

    [HttpPost("signin")]
    public async Task<ActionResult<AuthResponse>> SignIn([FromBody] SignInCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Ok(result);
    }

    [HttpPost("signout")]
    [Authorize]
    public new async Task<ActionResult> SignOut()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return BadRequest("Invalid user");
            
        var command = new SignOutCommand { UserId = userId };
        var result = await _mediator.Send(command);
        
        if (!result)
            return BadRequest("Sign out failed");
            
        return Ok(new { message = "Signed out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return BadRequest("Invalid user");

        var query = new GetCurrentUserQuery { UserId = userId };
        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
