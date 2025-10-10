using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userGuid))
            return null;

        var user = await _unitOfWork.Users.GetByIdAsync(userGuid);
        if (user == null)
            return null;

        return _mapper.Map<UserDto>(user);
    }
}
