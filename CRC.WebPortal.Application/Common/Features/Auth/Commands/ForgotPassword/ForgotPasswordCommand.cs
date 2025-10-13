using MediatR;
using CRC.WebPortal.Application.Common.Models;

namespace CRC.WebPortal.Application.Common.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
}
