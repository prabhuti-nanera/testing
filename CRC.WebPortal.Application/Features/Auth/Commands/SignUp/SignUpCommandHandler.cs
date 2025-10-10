using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Interfaces;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignUp;

/// <summary>
/// Pure business logic handler for user registration
/// No UI concerns, no validation - only business rules
/// </summary>
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthResponse>
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public SignUpCommandHandler(
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

    public async Task<AuthResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // Business Rule: Check if email already exists
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
        {
            return new AuthResponse { IsSuccess = false };
        }

        // Business Logic: Create new user
        var user = await CreateUserAsync(request);
        
        // Business Logic: Generate authentication tokens
        var authResult = await GenerateAuthenticationAsync(user);

        return authResult;
    }

    #endregion

    #region Private Business Logic Methods

    /// <summary>
    /// Business logic: Create and persist new user
    /// </summary>
    private async Task<User> CreateUserAsync(SignUpCommand request)
    {
        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            IsEmailVerified = false
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Business logic: Generate JWT and refresh tokens
    /// </summary>
    private async Task<AuthResponse> GenerateAuthenticationAsync(User user)
    {
        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Update user with refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
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
