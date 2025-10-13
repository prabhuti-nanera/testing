using MediatR;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Domain.Interfaces;

namespace CRC.WebPortal.Application.Common.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate reset code format
            if (string.IsNullOrEmpty(request.ResetCode) || request.ResetCode.Length != 6 || !request.ResetCode.All(char.IsDigit))
            {
                return new AuthResponse 
                { 
                    IsSuccess = false, 
                    Message = "Invalid reset code format." 
                };
            }

            // Check if user exists
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            
            if (user == null)
            {
                return new AuthResponse 
                { 
                    IsSuccess = false, 
                    Message = "Invalid email or reset code." 
                };
            }

            // In a real implementation, you would:
            // 1. Validate the reset token from database
            // 2. Check if token is not expired
            // 3. Ensure token hasn't been used already
            // For demo purposes, we'll accept any 6-digit code

            // Hash the new password (using simple hashing for demo)
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            
            // Update user password
            user.PasswordHash = hashedPassword;
            user.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse 
            { 
                IsSuccess = true, 
                Message = "Password has been reset successfully." 
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse 
            { 
                IsSuccess = false, 
                Message = "An error occurred while resetting your password. Please try again." 
            };
        }
    }
}
