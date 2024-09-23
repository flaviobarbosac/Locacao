using AutoMapper;
using Locacao.Domain.Model;
using Locacao.Dto;

namespace Locacao.API.Configuration
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Mapeamento de Product para ProductDto e vice-versa
            CreateMap<Motorcycle, MotorcycleDto>();
            CreateMap<MotorcycleDto, Motorcycle>();

            CreateMap<Deliveryman, DeliverymanDto>();
            CreateMap<DeliverymanDto, Deliveryman>();

            CreateMap<MotorcycleRegistrationEvent, MotorcycleRegistrationEventDto>();
            CreateMap<MotorcycleRegistrationEventDto, MotorcycleRegistrationEvent>();

            CreateMap<Rental, RentalDto>();
            CreateMap<RentalDto, Rental>();

            CreateMap<LoginDto, Login>();
            CreateMap<Login, LoginDto>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();            
        }
    }
}
