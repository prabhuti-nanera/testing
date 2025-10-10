namespace CRC.WebPortal.BlazorUI.Services;

/// <summary>
/// UI command model for sign-in operation to send to API
/// </summary>
public class SignInCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}
