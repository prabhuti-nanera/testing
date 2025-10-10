using CRC.WebPortal.Application.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}
