using Application.Eventos.DataTransferObjets;
using AutoMapper;
using Domain.Eventos.Entities;
using Domain.Eventos.ValueObjets;

namespace Application.Eventos.Mappings
{
    public class EventoMapping : Profile
    {
        public EventoMapping()
        {

            CreateMap<Evento, EventoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreyApellidoResponsable, opt => opt.MapFrom(src => src.NombreyApellidoResponsable))
                .ForMember(dest => dest.TelefonoResponsable, opt => opt.MapFrom(src => src.TelefonoResponsable))
                .ForMember(dest => dest.EmailResponsable, opt => opt.MapFrom(src => src.EmailResponsable))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Horario, opt => opt.MapFrom(src => src.Horario))
                .ForMember(dest => dest.SalaDtos, opt => opt.MapFrom(src => src.Salas))
                .ForMember(dest => dest.Recursos, opt => opt.MapFrom(src => src.Recursos));

            CreateMap<LogisticaMuestraTemporal, LogisticaMuestraTemporalDto>().ReverseMap();

            CreateMap<PropuestaMuestra, PropuestaMuestraDto>()
                .ForMember(dest => dest.MuestraDto, opt => opt.MapFrom(src => src.MuestraTemporal)); ;

            CreateMap<MuestraTemporal, MuestraDto>()
                .ForMember(dest => dest.MuestraId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreApellidoResponsable, opt => opt.MapFrom(src => src.NombreApellidoResponsable))
                .ForMember(dest => dest.TelefonoResponsable, opt => opt.MapFrom(src => src.TelefonoResponsable))
                .ForMember(dest => dest.EmailResponsable, opt => opt.MapFrom(src => src.EmailResponsable))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.EmailInstitucion, opt => opt.MapFrom(src => src.EmailInstitucion))
                .ForMember(dest => dest.TituloPropuesta, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.DescripcionMuestra, opt => opt.MapFrom(src => src.DescripcionMuestra))
                .ForMember(dest => dest.FundamentacionMuestra, opt => opt.MapFrom(src => src.FundamentacionMuestra))
                .ForMember(dest => dest.LogisticaMuestraTemporal, opt => opt.MapFrom(src => src.LogisticaMuestraTemporal))
                .ForMember(dest => dest.Publico, opt => opt.MapFrom(src => src.Publico))
                .ForMember(dest => dest.RequiereDifusion, opt => opt.MapFrom(src => src.RequiereDifusion))
                .ForMember(dest => dest.UrlImagenes, opt => opt.MapFrom(src => src.UrlImagenesMuestra))
                .ForMember(dest => dest.SolicitaFlyer, opt => opt.MapFrom(src => src.SolicitaFlyer))
                .ForMember(dest => dest.Horario, opt => opt.MapFrom(src => src.Horario))
                .ForMember(dest => dest.EstadoActividad, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.SalaDtos, opt => opt.MapFrom(src => src.Salas));


            CreateMap<PropuestaEvento, PropuestaEventoDto>()
                .ForMember(dest => dest.Evento, opt => opt.MapFrom(src => src.Evento)); ;





            CreateMap<RecursoAsignadoPropuesta, RecursoAsignadoPropuestaDto>();




            CreateMap<Evento, TablaReporteEventoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => src.TipoEvento.ToString()))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.DescripcionEvento))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.TipoPublico, opt => opt.MapFrom(src => src.TipoPublico.Select(tp => tp.ToString()).ToList()))
                .ForMember(dest => dest.Salas, opt => opt.MapFrom(src => src.Salas.Select(sala => sala.Nombre).ToList()))
                .ForMember(dest => dest.NombreSolicitante, opt => opt.MapFrom(src => src.NombreyApellidoResponsable))
                .ForMember(dest => dest.CantidadEstimada, opt => opt.MapFrom(src => src.CantidadEstimada ?? 0))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));

            CreateMap<Evento, EventoPorSalaDto>()
                 .ForMember(dest => dest.Salas, opt => opt.MapFrom(src => src.Salas.Select(sala => sala.Nombre).ToList()))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => src.TipoEvento.ToString()))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.TipoPublico, opt => opt.MapFrom(src => src.TipoPublico.Select(tp => tp.ToString()).ToList()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadEstimada ?? 0))
                .ForMember(dest => dest.Recursos, opt => opt.MapFrom(src => src.Recursos.Select(r => r.Recurso.NombreRecurso).ToList()));
        }
    }
}