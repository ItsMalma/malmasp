using AutoMapper;
using Malmasp.Dtos;
using Malmasp.Models;
using Malmasp.Services;

namespace Malmasp.Profiles;

public class UserProfile : Profile
{
    public UserProfile(HasherService hasherService)
    {
        CreateMap<UserRequestDto, User>()
            .ForMember(
                dest => dest.Password,
                opt => opt.MapFrom(
                    src => hasherService.HashPassword(src.Password)
                    )
                );
        CreateMap<User, UserResponseDto>();
    }
}