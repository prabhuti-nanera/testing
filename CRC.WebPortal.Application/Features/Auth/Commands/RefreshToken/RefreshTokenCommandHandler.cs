using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Interfaces;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResponse
            {
                IsSuccess = false
            };
        }

        var token = _tokenService.GenerateJwtToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = newRefreshToken,
            TokenExpiry = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user)
        };
    }
}
