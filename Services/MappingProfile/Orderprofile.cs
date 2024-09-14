using AutoMapper;
using Core.Entity.OrderEntity;
using Services.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public  class Orderprofile:Profile
    {
        public Orderprofile()
        {
            CreateMap<Order, OrderDto>()
               //.ForMember(d => d.UsertId, c => c.MapFrom(s => s.UserId))
               .ReverseMap();

            CreateMap<Order,OrderResult>()
                .ForMember(d=>d.SupTotal,c=>c.MapFrom(s=>s.Total));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, c => c.MapFrom(s=>s.Product.Id))
                .ForMember(d=>d.price,c=>c.MapFrom(d=>d.UnitPrice))
                .ForMember(d=>d.quantity,c=>c.MapFrom(s=>s.quantity))
                .ReverseMap();


            CreateMap<ShippingAddress, AddressDto>()
                .ReverseMap();


        }
    }
}
