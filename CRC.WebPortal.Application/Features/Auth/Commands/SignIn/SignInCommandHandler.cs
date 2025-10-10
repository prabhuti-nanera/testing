using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Interfaces;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignIn;

/// <summary>
/// Pure business logic handler for user authentication
/// No UI concerns, no validation - only business rules
/// </summary>
public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthResponse>
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public SignInCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    #endregion

    #region Handler Implementation

    public async Task<AuthResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        // Business Rule: Authenticate user
        var user = await AuthenticateUserAsync(request.Email, request.Password);
        if (user == null)
        {
            return new AuthResponse { IsSuccess = false };
        }

        // Business Rule: Check if user is active
        if (!user.IsActive)
        {
            return new AuthResponse { IsSuccess = false };
        }

        // Business Logic: Generate authentication and update user
        var authResult = await GenerateAuthenticationAsync(user, request.RememberMe);

        return authResult;
    }

    #endregion

    #region Private Business Logic Methods

    /// <summary>
    /// Business logic: Authenticate user credentials
    /// </summary>
    private async Task<Domain.Entities.User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email.ToLowerInvariant());
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }

    /// <summary>
    /// Business logic: Generate authentication tokens and update user session
    /// </summary>
    private async Task<AuthResponse> GenerateAuthenticationAsync(Domain.Entities.User user, bool rememberMe)
    {
        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Update user session information
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(rememberMe ? 30 : 7);
        user.LastLoginAt = DateTime.UtcNow;
        
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken,
            TokenExpiry = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user)
        };
    }

    #endregion
}
