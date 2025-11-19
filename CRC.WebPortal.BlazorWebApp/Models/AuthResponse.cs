namespace CRC.WebPortal.BlazorWebApp.Models;

<<<<<<< HEAD
/// Simple response model for authentication operations - UI layer only
=======
/// <summary>
/// Simple response model for authentication operations - UI layer only
/// </summary>
>>>>>>> d23dd02d994b72794089ec06b4a4ea15d34e4ff1
public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
<<<<<<< HEAD
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiry { get; set; }
=======
>>>>>>> d23dd02d994b72794089ec06b4a4ea15d34e4ff1
    public UserDto? User { get; set; }
}
