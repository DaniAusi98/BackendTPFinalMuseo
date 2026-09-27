using Application.ActividadesEducativas.DataTransferObjets;
using Application.ApplicationMuseo.DataTransferObjects;
using Application.ApplicationMuseo.DomainEvents;
using AutoMapper;
using Domain.ActividadesAreaEducacion.Entities;
using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ActividadesEducativas.Mappings
{
    public class Mapping : Profile

    {


        public Mapping()
        {
            CreateMap<EquipoTrabajoResponsable, EquipoResponsableDto>();
               

            CreateMap<ProyectosAreaEducacion, ProyectoEducativoDto>();

        }
    }
}
