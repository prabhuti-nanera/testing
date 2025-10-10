using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;

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
}