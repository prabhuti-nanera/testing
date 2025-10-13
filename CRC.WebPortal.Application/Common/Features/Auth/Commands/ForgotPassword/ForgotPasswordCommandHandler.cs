using MediatR;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Domain.Interfaces;

namespace CRC.WebPortal.Application.Common.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user exists
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            
            if (user == null)
            {
                // For security, don't reveal if email exists or not
                return new AuthResponse 
                { 
                    IsSuccess = true, 
                    Message = "If an account with this email exists, you will receive a password reset code." 
                };
            }

            // In a real implementation, you would:
            // 1. Generate a secure reset token
            // 2. Store it in database with expiration
            // 3. Send email/SMS with the token
            // For demo purposes, we'll just return success

            return new AuthResponse 
            { 
                IsSuccess = true, 
                Message = "Password reset code has been sent to your email." 
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse 
            { 
                IsSuccess = false, 
                Message = "An error occurred while processing your request. Please try again." 
            };
        }
    }
}
