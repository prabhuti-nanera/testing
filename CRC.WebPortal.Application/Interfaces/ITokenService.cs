using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Application.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
    string? GetUserIdFromToken(string token);
}
