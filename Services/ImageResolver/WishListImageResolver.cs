using AutoMapper;
using Core.Entity;
using Microsoft.Extensions.Configuration;
using Services.Dtos.WishListDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImageResolver
{
    public class WishListImageResolver : IValueResolver<WishList, WishListsDto, string>
    {
        private readonly IConfiguration confiq;

        public WishListImageResolver(IConfiguration confiq) 
        {
            this.confiq = confiq;
        }
        public string Resolve(WishList source, WishListsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.Image))
                 return confiq["BaseUrl"] + source.Product.Image;

            return null;
        }
    }
}
