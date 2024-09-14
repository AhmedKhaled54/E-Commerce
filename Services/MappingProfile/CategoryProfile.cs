using AutoMapper;
using Core.Entity;
using Services.Dtos.CatgegoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
             CreateMap<Category,CategoryDetailsDto>()
                .ForMember(s=>s.CategoryName,c=>c.MapFrom(d=>d.Name)).ReverseMap();
            CreateMap<Category,CreateOrUpdateCategory>()
                  .ForMember(s => s.CategoryName, c => c.MapFrom(d => d.Name)).ReverseMap();
        }
    }
}
