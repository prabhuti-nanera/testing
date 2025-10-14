namespace CRC.WebPortal.BlazorWebApp.Models;

/// <summary>
/// Simple response model for authentication operations - UI layer only
/// </summary>
public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UserDto? User { get; set; }
}
