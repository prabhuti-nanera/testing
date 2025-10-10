using FluentValidation;
using CRC.WebPortal.BlazorUI.Models;

namespace CRC.WebPortal.BlazorUI.Validators;

/// <summary>
/// UI validation for SignUp form - validates user input before sending to API
/// This is UI concern and belongs in BlazorUI layer
/// </summary>
public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    #region Constructor

    public SignUpRequestValidator()
    {
        ConfigureEmailValidation();
        ConfigurePasswordValidation();
        ConfigureNameValidation();
    }

    #endregion

    #region Private UI Validation Methods

    /// <summary>
    /// Configure email validation rules for UI
    /// </summary>
    private void ConfigureEmailValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Please enter a valid email address")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
    }

    /// <summary>
    /// Configure password validation rules for UI
    /// </summary>
    private void ConfigurePasswordValidation()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm your password")
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }

    /// <summary>
    /// Configure name validation rules for UI
    /// </summary>
    private void ConfigureNameValidation()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters")
            .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters")
            .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("Last name can only contain letters, spaces, hyphens, and apostrophes");
    }

    #endregion
}
