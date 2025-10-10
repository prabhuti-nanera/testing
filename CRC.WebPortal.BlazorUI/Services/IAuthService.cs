using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.BlazorUI.Models;

namespace CRC.WebPortal.BlazorUI.Services;

public interface IAuthService
{
    Task<UserDto> SignUpAsync(SignUpRequest request);
    // Other methods
}