using CRC.WebPortal.Application.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto?>
{
    public string UserId { get; set; } = string.Empty;
}
