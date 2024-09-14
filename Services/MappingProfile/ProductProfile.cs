using AutoMapper;
using Core.Entity;
using Services.Dtos.ProductsDto;
using Services.ImageResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
               .ForMember(d => d.ProductName, c => c.MapFrom(s => s.Name))
               .ForMember(d => d.Image, s => s.MapFrom<ProductImageresolver>())
               .ReverseMap();

            CreateMap<Product,ProductDetailsDto>()
                .ForMember(d => d.ProductName, c => c.MapFrom(s => s.Name))
               .ForMember(d => d.Brand, c => c.MapFrom(s => s.ProductBrand.Name))
               .ForMember(d => d.Type, c => c.MapFrom(s => s.ProductType.Name))
               .ForMember(d => d.Category, c => c.MapFrom(s => s.Category.Name))
               .ForMember(d => d.Image, s => s.MapFrom<ProductDeatailsImageResolver>())
               .ReverseMap();


            CreateMap<Product, CreateProductDto>()
                .ForMember(d => d.ProductName, c => c.MapFrom(s => s.Name))
                .ForMember(d=>d.Description,c=>c.MapFrom(s=>s.Description))
                .ForMember(d=>d.Price,c=>c.MapFrom(s=>s.Price))
                .ForMember(d => d.Productbrand_Id, c => c.MapFrom(s => s.ProductBrand.Id))
                .ForMember(d => d.productType_Id, c => c.MapFrom(s => s.ProductType.Id))
                .ForMember(d => d.CategoryId, c => c.MapFrom(s => s.Category.Id))
                .ForMember(d => d.Image, c => c.Ignore())
                .ReverseMap();
        }
    }
}
