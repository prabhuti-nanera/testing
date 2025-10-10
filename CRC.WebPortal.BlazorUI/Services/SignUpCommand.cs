namespace CRC.WebPortal.BlazorUI.Services;

/// <summary>
/// UI command model for sign-up operation to send to API
/// </summary>
public class SignUpCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
