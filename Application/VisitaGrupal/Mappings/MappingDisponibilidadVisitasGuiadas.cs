using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.DomainEvents;
using AutoMapper;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.ValueObjects;

namespace Application.VisitaGrupal.Mappings
{
    public class MappingVisitasGuiadas:Profile
    {
        public MappingVisitasGuiadas()
        {
            CreateMap<TurnoDisponible, TurnoDisponibleDto>()
                .ForMember(dest => dest.HorarioTurno, opt => opt.MapFrom(src => src.HorarioTurno))
                .ForMember(dest => dest.CuposDisponibles, opt => opt.MapFrom(src => src.CuposDisponibles))
                .ForMember(dest => dest.EstadoTurno, opt => opt.MapFrom(src => src.EstadoTurno.ToString()));
            CreateMap<SlotDisponibleVisitaAutoguiada, SlotDisponibleDto>()
                .ForMember(dest => dest.HorarioSlot, opt => opt.MapFrom(src => src.HorarioSlot))
                .ForMember(dest => dest.CapacidadMaximaSlot, opt => opt.MapFrom(src => src.CapacidadMaximaSlot))
                .ForMember(dest => dest.CuposGrupoDisponibles, opt => opt.MapFrom(src => src.CuposGrupoDisponibles))
                .ForMember(dest => dest.CapacidadMaximaPorGrupo, opt => opt.MapFrom(src => src.CapacidadMaximaPorGrupo))
                .ForMember(dest => dest.EstadoSlot, opt => opt.MapFrom(src => src.EstadoSlot.ToString()));
            CreateMap<BloqueoVisitasGuiadas, BloqueoVisitaGuiadaDto>()
                .ForMember(dest => dest.FechaDesde, opt => opt.MapFrom(src => src.FechaDesde))
                .ForMember(dest => dest.FechaHasta, opt => opt.MapFrom(src => src.FechaHasta))
                .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo));


            CreateMap<TematicaVisita, TematicaVisitaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Disponible, opt => opt.MapFrom(src => src.Disponible))
                .ForMember(dest => dest.SalaIds, opt => opt.MapFrom(src => src.Salas.Select(s => s.Id)));

            CreateMap<VisitaGrupalGuiada, GuidedTourReservationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioVisitanteId, opt => opt.MapFrom(src => src.UsuarioVisitanteId))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.EmailInstitucion, opt => opt.MapFrom(src => src.EmailInstitucion.Valor))
                .ForMember(dest => dest.PaisInstitucion, opt => opt.MapFrom(src => src.PaisInstitucion))
                .ForMember(dest => dest.ProvinciaInstitucion, opt => opt.MapFrom(src => src.ProvinciaInstitucion))
                .ForMember(dest => dest.CiudadInstitucion, opt => opt.MapFrom(src => src.LocalidadInstitucion))
                .ForMember(dest => dest.NivelCurso, opt => opt.MapFrom(src => src.NivelEducativo))
                .ForMember(dest => dest.AnioCurso, opt => opt.MapFrom(src => src.AnioGrado))
                .ForMember(dest => dest.DescripcionDiscapacidad, opt => opt.MapFrom(src => src.DiversidadFuncionalDescripcion))
                .ForMember(dest => dest.MotivoVisita, opt => opt.MapFrom(src => src.MotivoRelacionVisita))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.TematicasDto, opt => opt.MapFrom(src => src.Tematicas))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.EstadoConfirmacion, opt => opt.MapFrom(src => src.EstadoConfirmacion))
                .ForMember(dest => dest.Estado ,opt => opt.MapFrom(src => src.Estado));


            CreateMap<VisitaGrupalGuiada,VisitaGuiadaCreated>()
                .ForMember(dest => dest.VisitaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioVisitanteId, opt => opt.MapFrom(src => src.UsuarioVisitanteId))
                .ForMember(dest => dest.NombreInstitucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.NivelEducativo, opt => opt.MapFrom(src => src.NivelEducativo))
                .ForMember(dest => dest.AnioGrado, opt => opt.MapFrom(src => src.AnioGrado))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.Inicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.Fin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion));


            CreateMap<VisitaGrupalAutoguiada, VisitaAutoguiadaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioVisitanteId, opt => opt.MapFrom(src => src.UsuarioVisitanteId))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.EmailInstitucion, opt => opt.MapFrom(src => src.EmailInstitucion.Valor))
                .ForMember(dest => dest.PaisInstitucion, opt => opt.MapFrom(src => src.PaisInstitucion))
                .ForMember(dest => dest.ProvinciaInstitucion, opt => opt.MapFrom(src => src.ProvinciaInstitucion))
                .ForMember(dest => dest.CiudadInstitucion, opt => opt.MapFrom(src => src.LocalidadInstitucion))
                .ForMember(dest => dest.DescripcionDiscapacidad, opt => opt.MapFrom(src => src.DiversidadFuncional))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Horario.Inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.Horario.Fin))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.EstadoConfirmacion, opt => opt.MapFrom(src => src.EstadoConfirmacion))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

        }   
    }
}
