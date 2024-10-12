using AutoMapper;
using Core.Entity;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.WIshListsSpecification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Services.Dtos.Response;
using Services.Dtos.WishListDto;
using Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.WishListServices
{
    public class WishListServices : IWishListServices
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOf;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<User> userManager;

        public WishListServices (
            IMapper mapper,
            IUnitOfWork unitOf,
            IHttpContextAccessor httpContext,
            UserManager<User> userManager
            )
        {
            this.mapper = mapper;
            this.unitOf = unitOf;
            this.httpContext = httpContext;
            this.userManager = userManager;
        }




        public async  Task<ResponseDto> GetWishList()
        {
            var user=GetUserId();
            if (user == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Invalid User!"
                };
            }

            var wishlist = await unitOf.Repository<WishList>().GetAllPredicated(c => c.UserId == user,new[] {"Product"});
            if (wishlist!=null&& wishlist.Count()>0)
            {
                var map = mapper.Map<List<WishListsDto>>(wishlist);
                return new ResponseDto
                {
                    Status = 200,
                    IsSucceeded = true,
                    Models = map
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "There Is no product In Your WishList"
            };

        }

        public async Task<ResponseDto> AddWishList(AddORDeleteWIshListDto dto)
        {
            var response=new ResponseDto();

            var user = GetUserId();
            var product =await unitOf.Repository<Product>().FindAsync(c=>c.Id == dto.ProductId);

            var wishlist = await unitOf.Repository<WishList>()
                .FindAsync(w => w.UserId == user && w.productId == dto.ProductId);
            if (wishlist != null)
            {
                response.Status = 400;
                response.Message = "This Product has been Added Before!";
                return response;
            }

            var wish = new WishList
            {
                UserId = user,
                productId = dto.ProductId,
            };
            
            await unitOf.Repository<WishList>().AddAsync(wish);
            await unitOf.Complete();
            response.Status = 200;
            response.IsSucceeded=true;
            response.Message = "Product Added To Your WishList Successfuly";
            return response;
            
        }

        public async  Task<ResponseDto> RemoveWIshList(AddORDeleteWIshListDto dto)
        {
            var user=GetUserId();
            if (user == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Invalid User!"
                };
            }

            var wishlist=await unitOf.Repository<WishList>()
                .FindAsync(w=>w.UserId==user && w.productId == dto.ProductId);
            if (wishlist != null)
            {
                unitOf.Repository<WishList>().Delete(wishlist);
                await unitOf.Complete();
                return new ResponseDto
                {
                    Status = 200,
                    IsSucceeded = true,
                    Message = "product Delted From Wishlist Successfuly"
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "Product Deletion Faild!"
            };
           

        }


        private string GetUserId()
        {
            ClaimsPrincipal user = httpContext.HttpContext.User;
            var userid=userManager.GetUserId(user);
            return userid;
        }

        public async Task<Pagination<WishListsDto>> GetPagination(WishListSpecification specification)
        {
            var userid = GetUserId();

            var wish = await unitOf.Repository<WishList>()
                .GetAllPredicated(c => c.UserId == userid, new[] { "Product" });
         
            var specs=new WishListWithProductSecification(specification);
            var wishlist=await unitOf.Repository<WishList>().GetAllEntityWithSpecs(specs);
            var count = await unitOf.Repository<WishList>().GetCount(specs);
            var map=mapper.Map<List<WishListsDto>>(wishlist);
            return new Pagination<WishListsDto>(specification.PageSize, specification.PageIndex, count, map);

        }
    }
}
