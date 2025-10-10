using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignOut;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SignOutCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userGuid))
            return false;

        var user = await _unitOfWork.Users.GetByIdAsync(userGuid);
        if (user == null)
            return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
