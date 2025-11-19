namespace CRC.WebPortal.BlazorWebApp.Models;

<<<<<<< HEAD

/// Simple user data structure for UI layer - contains only what UI needs to display
/// This is NOT the domain entity, just a data structure for API communication
=======
/// <summary>
/// Simple user data structure for UI layer - contains only what UI needs to display
/// This is NOT the domain entity, just a data structure for API communication
/// </summary>
>>>>>>> d23dd02d994b72794089ec06b4a4ea15d34e4ff1
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
