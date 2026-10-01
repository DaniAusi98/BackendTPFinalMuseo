using Application.ActividadesEducativas.DataTransferObjets;
using AutoMapper;
using Domain.ActividadesAreaEducacion.Entities;

namespace Application.ActividadesEducativas.Mappings
{
    public class Mapping : Profile

    {


        public Mapping()
        {
            CreateMap<EquipoTrabajoResponsable, EquipoResponsableDto>();


            CreateMap<ProyectosAreaEducacion, ProyectoEducativoDto>();

            CreateMap<ActividadEducativa, ActividadEducativaDto>()

                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.TipoActividadEducativa, opt => opt.MapFrom(src => src.TipoActividadEducativa.ToString()))
                .ForMember(dest => dest.ProyectoVinculado, opt => opt.MapFrom(src => src.ProyectoVinculada))
                .ForMember(dest => dest.InstitucionVinculada, opt => opt.MapFrom(src => src.InstitucionVinculada))
                .ForMember(dest => dest.PublicoObjetivo, opt => opt.MapFrom(src => src.PublicoObjetivo))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => src.Horario))
                .ForMember(dest => dest.Salas, opt => opt.MapFrom(src => src.Salas))
                .ForMember(dest => dest.Recursos, opt => opt.MapFrom(src => src.Recursos));
        }
    }
}
