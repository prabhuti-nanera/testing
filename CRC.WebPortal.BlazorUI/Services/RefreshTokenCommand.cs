namespace CRC.WebPortal.BlazorUI.Services;

/// <summary>
/// UI command model for refresh token operation to send to API
/// </summary>
public class RefreshTokenCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}
