using AutoMapper;
using Core.Entity;
using Core.Entity.CartEntity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Services.Dtos.CartsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImageResolver
{
    public class CartImageResolver : IValueResolver<CartItem, CartItemDto, string>
    {
        private readonly IConfiguration config;

        public CartImageResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(CartItem source, CartItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.Image))
                return
                    config["BaseUrl"] +source.Product.Image;

            return null;

        }
    }
}
