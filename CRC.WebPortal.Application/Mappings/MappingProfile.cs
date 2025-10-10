using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
    }
}