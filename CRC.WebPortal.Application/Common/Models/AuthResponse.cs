namespace CRC.WebPortal.Application.Common.Models;

/// <summary>
/// Standard response model for authentication operations
/// </summary>
public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiry { get; set; }
    public UserDto? User { get; set; }
}
