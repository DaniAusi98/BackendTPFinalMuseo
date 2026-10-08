using Application.PersonalMuseo.DataTrasnferObjets;
using AutoMapper;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Application.PersonalMuseo.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Area, AreaDto>();




        }
    }
}