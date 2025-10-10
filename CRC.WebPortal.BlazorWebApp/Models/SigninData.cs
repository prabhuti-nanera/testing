using System.ComponentModel.DataAnnotations;

namespace CRC.WebPortal.BlazorWebApp.Models;

/// <summary>
/// Simple data structure for signin form - contains only UI data for API transfer
/// </summary>
public class SigninData
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}
