using CRC.WebPortal.BlazorUI.Models;

namespace CRC.WebPortal.BlazorUI.Services;

public interface IAuthService
{
    Task<UserDto> SignInAsync(string email, string password);
    Task<UserDto> SignUpAsync(string email, string password, string firstName, string lastName);
    Task<AuthResponse> RefreshTokenAsync();
    Task<AuthResponse> ForgotPasswordAsync(object request);
    Task<AuthResponse> ResetPasswordAsync(object request);
    Task SignOutAsync();
    Task<UserDto?> GetCurrentUserAsync();
}
