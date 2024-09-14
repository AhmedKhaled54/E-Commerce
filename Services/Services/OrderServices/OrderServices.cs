using AutoMapper;
using Core.Entity;
using Core.Entity.CartEntity;
using Core.Entity.OrderEntity;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.CartSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.OrderDto;
using Services.Dtos.Response;
using Services.HandleResponse;
using Services.Services.CartServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.OrderServices
{
    public  class OrderServices:IOrderServices
    {
        private readonly IUnitOfWork unitOf;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<User> userManager;
        private readonly ICartServices cartServices;

        public OrderServices(IUnitOfWork unitOf,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            UserManager<User> userManager,
            ICartServices cartServices)
        {
            this.unitOf = unitOf;
            this.mapper = mapper;
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.cartServices = cartServices;
        }

        public async Task<ResponseDto> CreateOrder(OrderDto orderDto)
        {

            var response = new ResponseDto();
            var user = GetUserId();
            if (user == null)
            {
                response.Status = 400;
                response.Message = "Invalid User Please Try Again !";
                return response;
            }
            var cart = await cartServices.GetCartforuser(user);
            var cartwithcartitem = await GetCartSpecs();

            if (cart == null || !cartwithcartitem.CartItems.Any())
            {
                response.Status = 400;
                response.Message = " Please try Again (Select at least one product)";
                return response;
            }

            var Address = mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
            var orderitems = new List<OrderItem>();
            foreach (var item in cart.CartItems)
            {
                var product = await unitOf.Repository<Product>().FindAsync(c => c.Id == item.productId);

                var orderitem = new OrderItem
                {
                    productId = product.Id,
                    UnitPrice = item.Product.Price,
                    quantity = item.Quantity,

                };
                orderitems.Add(orderitem);
            }

            var suptotal = orderitems.Sum(c => c.quantity * c.UnitPrice);

            var order = new Order
            {
                UserId = user,
                CreatedAt = DateTime.Now,
                ShippingAddress = Address,
                OrderItems = orderitems,
                Total = suptotal,

            };
            if (order != null)
            {
                await unitOf.Repository<Order>().AddAsync(order);
            }
            
            await cartServices.RemoveItemBecomeOrder(user);
            var OrderResult=mapper.Map<OrderResult>(order);
            var mapOrderitem = mapper.Map<List<OrderItemDto>>(orderitems);
            OrderResult.Items=mapOrderitem;
            await unitOf.Complete();

            response.Model = OrderResult;
            response.IsSucceeded = true;
            return response;
            
        }



        private  string GetUserId()
        {
            ClaimsPrincipal user = httpContext.HttpContext.User;
            var userid = userManager.GetUserId(user);
            return userid;
        }


        private async Task<Cart> GetCartSpecs()
        {
            var specification = new CartSpecification();
            var specs=new CartWithCartItemSpecification(specification);
            var cart=await unitOf.Repository<Cart>().GetEntityWithSpecs(specs);
            return cart;
        }
    }
}
