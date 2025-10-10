using System.ComponentModel.DataAnnotations;

namespace CRC.WebPortal.BlazorUI.Models;

public class SignUpRequest
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
