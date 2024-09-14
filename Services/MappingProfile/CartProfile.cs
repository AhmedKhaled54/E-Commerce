using AutoMapper;
using Core.Entity.CartEntity;
using Services.Dtos.CartsDto;
using Services.ImageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
             CreateMap<Cart,CartDto>()
                .ForMember(d=>d.CartId,c=>c.MapFrom(s=>s.Id)).ReverseMap();

            CreateMap<CartItem, CartItemDto>()
                .ForMember(d => d.ProductId, c => c.MapFrom(s => s.Product.Id))
                .ForMember(d => d.ProductName, c => c.MapFrom(s => s.Product.Name))
                .ForMember(d => d.Description, c => c.MapFrom(s => s.Product.Description))
                .ForMember(d => d.Price, c => c.MapFrom(s => s.Product.Price))
                .ForMember(d=>d.Image,c=>c.MapFrom<CartImageResolver>())
                .ReverseMap();
        }
    }
}
