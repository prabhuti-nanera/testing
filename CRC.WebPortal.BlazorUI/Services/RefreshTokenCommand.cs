namespace CRC.WebPortal.BlazorUI.Services;

/// UI command model for refresh token operation to send to API
public class RefreshTokenCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}
