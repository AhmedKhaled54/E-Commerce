using AutoMapper;
using Core.Entity;
using Microsoft.Extensions.Configuration;
using Services.Dtos.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImageResolver
{
    public class ProductDeatailsImageResolver : IValueResolver<Product, ProductDetailsDto, string>
    {
        private readonly IConfiguration confiq;

        public ProductDeatailsImageResolver(IConfiguration confiq)
        {
            this.confiq = confiq;
        }
        public string Resolve(Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Image))
                return confiq["BaseUrl"] + source.Image;

            return null;
        }
    }
}
