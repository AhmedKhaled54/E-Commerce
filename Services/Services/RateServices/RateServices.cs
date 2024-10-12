using Core.Entity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Services.Dtos.RateDtos;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.RateServices
{
    public  class RateServices:IRateServices 
    {
        private readonly IUnitOfWork unitOf;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<User> userManager;

        public RateServices(IUnitOfWork unitOf,
            IHttpContextAccessor httpContext,
            UserManager<User> userManager)
        {
            this.unitOf = unitOf;
            this.httpContext = httpContext;
            this.userManager = userManager;
        }

        public async Task<ResponseDto> AddRate(AddRateDto dto)
        {
            var user = GetUserId();
            var product =await unitOf.Repository<Product>().FindAsync(c=>c.Id== dto.ProductId);
            if (product == null)
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Invalid Product.. Please Try Again !"
                };
            var rate = new Rate
            {
                UserId = user,
                ProductId = product.Id,
                rate = dto.Rate,
                Message = dto.Message,
            };
            await unitOf.Repository<Rate>().AddAsync(rate);
            await unitOf.Complete();
            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Added Your Rating Successfuly "
            };



        }

        public async Task<double> GetavarageForProduct(int ProductId)
        {
            var rate = await unitOf.Repository<Rate>().GetAllPredicated(c=>c.ProductId== ProductId);

            return rate.Any() ?(double)rate.Average(r => r.rate) : 0;
        }

        public async Task<ResponseDto> GetProductsRate(int ProductId)
        {
            var Rate =await unitOf.Repository<Rate>().GetAllPredicated(c=>c.ProductId== ProductId, new[] {"Product","User"} );

            var count=Rate.Count();
            var productRate=new ProductRatedto { Count=count };
            decimal ?sum = 0;

            var userRate = new List<UserRateDto>();
            if (count == 0)
            {
                productRate.Count = 0;
                productRate.Avarage = 0;

                return new ResponseDto
                {
                    Status = 400,
                    Model = productRate,
                };
            }

            foreach(var r in Rate)
            {
                var result = new UserRateDto
                {
                    Rate = r.rate,
                    Comment = r.Message,
                    UserName = r.User.UserName
                };
                sum += result.Rate;
                userRate.Add(result);

            }
            var Avarage =sum/productRate.Count;
            productRate.Avarage=Avarage;
            productRate.CustomerRate = userRate;

            return new ResponseDto
            {
                IsSucceeded = true,
                Model = productRate,
                Status = 200

            };




        }

        private string GetUserId()
        {
            var user = httpContext.HttpContext.User;
            var userid = userManager.GetUserId(user);
            return userid;
        }
    }
}
