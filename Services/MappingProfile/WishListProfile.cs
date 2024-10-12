using AutoMapper;
using Core.Entity;
using Services.Dtos.WishListDto;
using Services.ImageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public  class WishListProfile:Profile
    {
        public WishListProfile()
        {
            CreateMap<WishList, WishListsDto>()
               .ForMember(d => d.ProductId, c => c.MapFrom(s => s.Product.Id))
               .ForMember(d => d.ProductName, c => c.MapFrom(s => s.Product.Name))
               .ForMember(d => d.price, c => c.MapFrom(s => s.Product.Price))
               .ForMember(d => d.Description, c => c.MapFrom(s => s.Product.Description))
               .ForMember(d=>d.Image,c=>c.MapFrom<WishListImageResolver>()).ReverseMap();
        }
    }
}
