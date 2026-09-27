using Application.ActividadMuseo.DataTransferObjets;

using AutoMapper;

using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;

namespace Application.ActividadMuseo.Mappings
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<TimeSlot, ActividadHorarioDto>()
               .ForMember(dest => dest.Inicio, opt => opt.MapFrom(src => src.Inicio))
               .ForMember(dest => dest.Fin, opt => opt.MapFrom(src => src.Fin));

            CreateMap<RecursoAsignado,ActividadRecursoDto>()
                .ForMember(dest => dest.RecursoId, opt => opt.MapFrom(src => src.RecursoId))
                .ForMember(dest => dest.CantidadAsignada, opt => opt.MapFrom(src => src.CantidadAsignada))
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso.NombreRecurso))
                .ForMember(dest => dest.TipoRecurso, opt => opt.MapFrom(src => src.Recurso.TipoRecurso.ToString()));

            CreateMap<Sala,ActividadSalaDto>()
                .ForMember(dest => dest.SalaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreSala, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.TipoSala, opt => opt.MapFrom(src => src.TipoSala.ToString()))
                .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion.ToString()));

            CreateMap<Domain.ActividadMuseo.Entities.ActividadMuseo,ActividadMuseoDto>()
                .ForMember(dest => dest.TipoActividad, opt => opt.MapFrom(src => src.TipoActividad.ToString()))
                .ForMember(dest => dest.CategoriaActividad, opt => opt.MapFrom(src => src.CategoriaActividad.ToString()))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.TimeSlots, opt => opt.MapFrom(src => src.Horario))
                .ForMember(dest => dest.Salas, opt => opt.MapFrom(src => src.Salas))
                .ForMember(dest => dest.Recursos, opt => opt.MapFrom(src => src.Recursos));

        }
    }
}
