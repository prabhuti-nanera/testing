namespace CRC.WebPortal.BlazorWebApp.Models;

public class LogoutData
{
    // For logout, we typically don't need any data from the UI
    // The token will be handled by the service layer
    public string? Reason { get; set; } = "User initiated logout";
}
