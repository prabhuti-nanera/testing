using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.BlazorWebApp.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

public interface IAuthService
{
    Task<AuthResponse> SignUpAsync(SignupData signupData);
    Task<AuthResponse> SignInAsync(SigninData signinData);
}