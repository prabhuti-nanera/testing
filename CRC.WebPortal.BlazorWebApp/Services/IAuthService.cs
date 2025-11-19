using CRC.WebPortal.BlazorWebApp.Models;
using CRC.Common.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

public interface IAuthService
{
<<<<<<< HEAD
    Task<AuthResponse> SignUpAsync(SignupRequest signupData);
    Task<AuthResponse> SignInAsync(SigninRequest signinData);
    Task<AuthResponse> SendOtpAsync(SendOtpRequest request);
    Task<AuthResponse> VerifyOtpAndResetPasswordAsync(VerifyOtpRequest request);
=======
    Task<AuthResponse> SignUpAsync(SignupData signupData);
    Task<AuthResponse> SignInAsync(SigninData signinData);
>>>>>>> d23dd02d994b72794089ec06b4a4ea15d34e4ff1
    Task LogoutAsync();
}
