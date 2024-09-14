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
    public class ProductImageresolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _confiq;

        public ProductImageresolver(IConfiguration confiq)
        {
            this._confiq = confiq;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Image))
               return _confiq["BaseUrl"] + source.Image;

            return null;

        }
    }
}
