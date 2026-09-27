using Application.ApplicationMuseo.DataTransferObjects;
using Application.ApplicationMuseo.DomainEvents;

using AutoMapper;

using Domain.Common.Entities;
using Domain.Common.ValueObjets;

namespace Application.ApplicationMuseo.Mappings
{
    /// <summary>
    /// El mapeo entre objetos debe ir definido aqui
    /// </summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DummyEntity, DummyEntityCreated>().ReverseMap();
            CreateMap<DummyEntity, DummyEntityUpdated>().ReverseMap();
            CreateMap<DummyEntity, DummyEntityDto>().ReverseMap();

            // TimeSlot es un ValueObject, por lo que no se mapea directamente a un DTO, sino que se mapea a sus propiedades individuales
            CreateMap<TimeSlot, TimeSlotDto>()
               .ForMember(dest => dest.Inicio, opt => opt.MapFrom(src => src.Inicio))
               .ForMember(dest => dest.Fin, opt => opt.MapFrom(src => src.Fin));

            CreateMap<Provincia, ProvinciaDto>().ReverseMap();
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<LocalidadArg, LocalidadDto>().ReverseMap();

        }
    }
}
