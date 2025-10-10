using FluentValidation;
using CRC.WebPortal.BlazorUI.Models;

namespace CRC.WebPortal.BlazorUI.Validators;

/// <summary>
/// UI validation for SignIn form - validates user input before sending to API
/// This is UI concern and belongs in BlazorUI layer
/// </summary>
public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    #region Constructor

    public SignInRequestValidator()
    {
        ConfigureEmailValidation();
        ConfigurePasswordValidation();
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
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters");
    }

    #endregion
}
