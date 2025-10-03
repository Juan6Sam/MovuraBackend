using AutoMapper;
using Movura.Domain.Entities;
using Movura.Api.Models.Dto;

namespace Movura.Api.Services.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Email));
        CreateMap<Parking, ParkingDto>();
        CreateMap<ParkingConfig, ParkingConfigDto>();
        CreateMap<Comercio, ComercioDto>();
        CreateMap<Ticket, TicketDto>();
        CreateMap<Transaccion, TransactionDto>();

        // Mapeos inversos para crear/actualizar
        CreateMap<ParkingConfigDto, ParkingConfig>();
        CreateMap<ComercioDto, Comercio>();
    }
}