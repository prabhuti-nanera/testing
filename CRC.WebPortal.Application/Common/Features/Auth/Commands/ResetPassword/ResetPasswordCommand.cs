using MediatR;
using CRC.WebPortal.Application.Common.Models;

namespace CRC.WebPortal.Application.Common.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string ResetCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
