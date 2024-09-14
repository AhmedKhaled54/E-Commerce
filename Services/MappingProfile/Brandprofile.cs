using AutoMapper;
using Core.Entity;
using Services.Dtos.BrandsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public  class Brandprofile:Profile
    {
        public Brandprofile()
        {
             CreateMap<ProductBrand,BrandDto>()
                .ForMember(s=>s.BrandName,c=>c.MapFrom(d=>d.Name))
                .ReverseMap();

            CreateMap<ProductBrand,CreateOrUpdateBrandDto>()
                .ForMember(s=>s.BrandName,c=>c.MapFrom(d=>d.Name))
                .ReverseMap();
        }
    }
}
