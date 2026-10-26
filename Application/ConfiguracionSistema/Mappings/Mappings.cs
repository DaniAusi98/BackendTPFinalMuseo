using Application.ConfiguracionSistema.DataTransferObjets;
using AutoMapper;
using Domain.Common.Entities;
using Domain.Common.ValueObjets;

namespace Application.ConfiguracionSistema.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {

            CreateMap<HorarioApertura, HorarioAperturaDto>()
                .ForMember(dest => dest.Dia, opt => opt.MapFrom(src => src.Dia))
                .ForMember(dest => dest.Desde, opt => opt.MapFrom(src => src.Desde))
                .ForMember(dest => dest.Hasta, opt => opt.MapFrom(src => src.Hasta));

            CreateMap<DiaCierreMuseo, DiaCierreDto>()

                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo.ToString()))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones));



        }
    }
}


