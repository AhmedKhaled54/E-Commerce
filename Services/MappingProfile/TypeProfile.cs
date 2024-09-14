using AutoMapper;
using Core.Entity;
using Services.Dtos.TypeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public  class TypeProfile:Profile
    {
        public TypeProfile()
        {
             CreateMap<ProductType,TypeDto>()
                .ForMember(s=>s.TypeName,c=>c.MapFrom(d=>d.Name)).ReverseMap();
            CreateMap<ProductType,CreateOrUpdateTypeDto>()
                .ForMember(s => s.TypeName, c => c.MapFrom(d => d.Name)).ReverseMap();

        }
    }
}
