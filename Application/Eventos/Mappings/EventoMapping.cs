using Application.Eventos.DataTransferObjets;
using AutoMapper;
using Domain.Eventos.Entities;

namespace Application.Eventos.Mappings
{
    public class EventoMapping : Profile
    {
        public EventoMapping()
        {
            CreateMap<Evento, TablaReporteEventoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => src.TipoEvento.ToString()))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.TituloEvento))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.DescripcionEvento))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.TipoPublico, opt => opt.MapFrom(src => src.TipoPublico.Select(tp => tp.ToString()).ToList()))
                .ForMember(dest => dest.Salas, opt => opt.MapFrom(src => src.Salas.Select(sala => sala.Nombre).ToList()))
                .ForMember(dest => dest.NombreSolicitante, opt => opt.MapFrom(src => src.NombreyApellidoSolicitante))
                .ForMember(dest => dest.CantidadEstimada, opt => opt.MapFrom(src => src.CantidadEstimada ?? 0))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));

            CreateMap<Evento, EventoPorSalaDto>()
                 .ForMember(dest => dest.Salas, opt => opt.MapFrom(src => src.Salas.Select(sala => sala.Nombre).ToList()))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => src.TipoEvento.ToString()))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.TituloEvento))
                .ForMember(dest => dest.TipoPublico, opt => opt.MapFrom(src => src.TipoPublico.Select(tp => tp.ToString()).ToList()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadEstimada ?? 0))
                .ForMember(dest => dest.Recursos, opt => opt.MapFrom(src => src.Recursos.Select(r => r.Recurso.NombreRecurso).ToList()));
        }
    }
}