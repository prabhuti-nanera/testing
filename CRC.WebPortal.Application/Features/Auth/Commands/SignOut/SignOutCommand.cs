using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignOut;

public class SignOutCommand : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
}
