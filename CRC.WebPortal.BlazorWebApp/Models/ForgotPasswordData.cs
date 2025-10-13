using System.ComponentModel.DataAnnotations;

namespace CRC.WebPortal.BlazorWebApp.Models;

public class ForgotPasswordData
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
}
