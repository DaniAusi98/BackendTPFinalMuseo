/*using Application.Usuario.DataTransferObjets;
using Application.Usuario.DomainEvents;
using AutoMapper;
using Domain.CommonDomain.ValueObjets;

namespace Application.Usuario.Mappings
{
    public class UsuarioMapping : Profile
    {
        public UsuarioMapping()
        {
            // UsuarioVisitante -> UsuarioVisitanteCreado


            CreateMap<Visitante, UsuarioVisitanteCreado>()
           .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
           .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailVisitante.Valor))
           .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.TelefonoVisitante.Valor))
           .ForMember(dest => dest.UsuarioVisitanteId, opt => opt.MapFrom(src => src.Id));


            //Login Response

            CreateMap<Visitante, UserDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // 👈 nuevo
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre))
              .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Apellido))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailVisitante.Valor))
             .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TelefonoVisitante.Valor));

            CreateMap<TimeSlot, TimeSlotDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Inicio))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Fin));





            CreateMap<TimeSlot, HorarioDto>();




            // === UsuarioVisitante DTO ===

            CreateMap<Visitante, UsuarioVisitanteDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailVisitante.Valor))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.TelefonoVisitante.Valor));

            CreateMap<UsuarioVisitanteDto, Visitante>()
           .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src =>src.Nombre))
           .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src =>src.Apellido))
           .ForMember(dest => dest.EmailVisitante, opt => opt.MapFrom(src => new Email(src.Email)))
           .ForMember(dest => dest.TelefonoVisitante, opt => opt.MapFrom(src => new Telefono(src.Telefono)));







        }



    }
}
*/
