using Application.PersonalMuseo.DataTrasnferObjets;
using AutoMapper;

namespace Application.PersonalMuseo.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {

            CreateMap<Area, AreaDto>()
            .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));




            CreateMap<Puesto, PuestoDto>()
            .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));


            CreateMap<AreaPuesto, AreaPuestoDto>()
                  .ForMember(dest => dest.AreaPuestoId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId))
                 .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                 .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.PuestoId))
                 .ForMember(dest => dest.Puesto, opt => opt.MapFrom(src => src.Puesto));

            CreateMap<AsignacionPersonal, AsignacionPersonalDto>()
            .ForMember(dest => dest.AsignacionPersonalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AreaPuestoId, opt => opt.MapFrom(src => src.AreaPuestoId))
            .ForMember(dest => dest.AreaPuesto, opt => opt.MapFrom(src => src.AreaPuesto))
            .ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(src => src.FechaAsignacion))
            .ForMember(dest => dest.Activa, opt => opt.MapFrom(src => src.Activa));


            CreateMap<Personal, PersonalDto>()
                .ForMember(dest => dest.Asignaciones,
                    opt => opt.MapFrom(src => src.Asignaciones
                        .Where(a => a.Activa)
                        .ToList()));










        }
    }
}