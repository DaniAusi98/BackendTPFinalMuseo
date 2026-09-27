using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.MuseumResources.DataTransferObjects;
using Application.VisitaGrupal.DataTransferObjets;

using AutoMapper;

using Domain.RecursoMuseo.Entities;
using Domain.VisitasGrupales.Entities;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.Mappings
{
    public class MappingRecurso : Profile
    {
        public MappingRecurso()
        {
            CreateMap<Recurso, RecursoDto>()
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.NombreRecurso))

                .ForMember(dest => dest.TipoRecurso, opt => opt.MapFrom(src => src.TipoRecurso.ToString()))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()))
                .ForMember(dest => dest.CantidadTotal, opt => opt.MapFrom(src => src.CantidadTotal));

            /*CreateMap<RecursoDto, Recurso>()
                  .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.NombreRecurso))
                  .ForMember(dest => dest.TipoRecurso, opt => opt.MapFrom(src => Enum.Parse<TipoRecurso>(src.TipoRecurso)))
                  .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                  .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => Enum.Parse<EstadoRecurso>(src.Estado)))
                  .ForMember(dest => dest.CantidadTotal, opt => opt.MapFrom(src => src.CantidadTotal));*/
            CreateMap<Sala, SalaDto>()
                  .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                  .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.EstadoSala.ToString()))
                  .ForMember(dest => dest.CodigoSala, opt => opt.MapFrom(src => src.CodigoSala))
                  .ForMember(dest => dest.Capacidad, opt => opt.MapFrom(src => src.Capacidad))
                  .ForMember(dest => dest.TipoSala, opt => opt.MapFrom(src => src.TipoSala.ToString()))
                  .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion.ToString()));
            CreateMap<TematicaVisita,TematicaVisitaDto>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Disponible, opt => opt.MapFrom(src => src.Disponible.ToString()));

        }
    }
}
/*CreateMap<TurnoDisponible, TurnoDisponibleDto>()
                .ForMember(dest => dest.HorarioTurno, opt => opt.MapFrom(src => src.HorarioTurno))
                .ForMember(dest => dest.CapacidadMaxima, opt => opt.MapFrom(src => src.CapacidadMaxima))
                .ForMember(dest => dest.CapacidadDisponible, opt => opt.MapFrom(src => src.CapacidadDisponible))
                .ForMember(dest => dest.EstadoTurno, opt => opt.MapFrom(src => src.EstadoTurno.ToString()));
*/
