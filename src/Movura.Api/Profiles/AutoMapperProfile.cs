using AutoMapper;
using Movura.Api.Data.Entities;
using Movura.Api.Models.Dto;

namespace Movura.Api.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();

        CreateMap<Comercio, ComercioDto>();
        CreateMap<ComercioDto, Comercio>();
    }
}
