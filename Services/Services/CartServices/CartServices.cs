using AutoMapper;
using Core.Data;
using Core.Entity;
using Core.Entity.CartEntity;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.CartItemSpecifications;
using Infrastructure.Specifications.CartSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.CartsDto;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.CartServices
{
    public class CartServices : ICartServices
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOf;
        private readonly AppDBContext context;

        public CartServices(
            IHttpContextAccessor  httpContextAccessor,
            IMapper mapper,
            UserManager<User> userManager,
            IUnitOfWork unitOf,
            AppDBContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
            this.unitOf = unitOf;
            this.context = context;
        }
        public async  Task<ResponseDto> AddCart(int productid, int quantity)
        {
            var response = new ResponseDto ();

            var user = getuserid();
            if (user == null)
            {
                response.Status = 400;
                response.Message = "Invalid User Please try Again! ";
                return response;
            }

            var cart =await  GetCartforuser(user);
            if (cart == null)
            {
                var Cart = new Cart
                {
                    UserId = user
                };
                await unitOf.Repository<Cart>().AddAsync(Cart);
                await unitOf.Complete();
            }


            var CartItem=await unitOf.Repository<CartItem>()
                .FindAsync(c=>c.CartId==cart.Id&&c.productId==productid);

            if (CartItem != null)
            {
                CartItem.Quantity += quantity;
            }
            else
            {
                var item = new CartItem
                {
                    productId = productid,
                    CartId = cart.Id,
                    Quantity = quantity
                };
                await unitOf.Repository<CartItem>().AddAsync(item);
              
            }
            await unitOf.Complete();
            response.IsSucceeded = true;
            response.Message = "Added product To Cart Successfuly ";
            response.Status = 200;
            return response;

        }

        public async Task<ResponseDto> GetCart()
        {
            var user=getuserid();
            if (user == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Invalid User Please try Again !"
                };
            }

            var products = await unitOf.Repository<Product>().GetAll();
            //var cart = await GetCartspecs();

            var cart =  unitOf.Repository<Cart>().GetEntityPredicated(c => c.UserId == user, new[] { "CartItems" });
            var MapCart=mapper.Map<CartDto>(cart);
            var cartitem = await GetCartItems();
            

            var MapCartItem =mapper.Map<List<CartItemDto>>(cartitem);
          
            MapCart.CartItems = MapCartItem;
            
            return new ResponseDto
            {
                Status=200,
                IsSucceeded=true,
                Model=MapCart
            };


        }

        public async Task<ResponseDto> RemoveproductfromCart(int productid, int quantity)
        {

            var response = new ResponseDto();
            var user = getuserid();
            if (user == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Invalid User Please try Again!"
                };
            }

            var cart=await GetCartforuser(user);
            var cartitems = await unitOf.Repository<CartItem>()
                .FindAsync(c => c.CartId == cart.Id && c.productId == productid);
            if (cartitems == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "No product In Your Cart!"
                };
            }

            if (cartitems.Quantity == 1)
            {
                unitOf.Repository<CartItem>().Delete(cartitems);
                response.Message = "Product Removed In Your Cart";
                response.Status = 200;
                response.IsSucceeded = true;
            }
            else if (cartitems.Quantity > quantity)
            {
                cartitems.Quantity = cartitems.Quantity - quantity;
                unitOf.Repository<CartItem>().Update(cartitems);
                response.Message = "Product Removed In Your Cart";
                response.Status = 200;
                response.IsSucceeded= true;
            }
            else 
            { 
                response.Message = "Incorrect System!";
                response.Status = 400;
            }
            await unitOf.Complete();
            return response;

        }
       public  async Task<Cart> GetCartforuser(string userid)
        {
            var cart =await unitOf.Repository<Cart>().FindAsync(c=>c.UserId== userid);
            return cart;
        }

        private string getuserid()
        {
            ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
            var userid= userManager.GetUserId(user);
            return userid;

        }

        private async Task<Cart> GetCartspecs()
        {
            var specification = new CartSpecification();
            var specs =new CartWithCartItemSpecification(specification);
            var carts=await unitOf.Repository<Cart>().GetEntityWithSpecs(specs);
            return carts;
        }

        private async Task<IEnumerable<CartItem>> GetCartItems()
        {
            var specification = new CartItemSpecification();
            var specs =new CartItemWithProductSpecfication(specification);
            var cartitem = await unitOf.Repository<CartItem>().GetAllEntityWithSpecs(specs);
            return cartitem;
        }

        public async Task RemoveItemBecomeOrder(string userid)
        {
           var cart=await context.Carts
                .Include(c=>c.CartItems)
                .Where(c=>c.UserId == userid)
                .FirstOrDefaultAsync();

            if (cart!=null)
            {
                context.CartItems.RemoveRange(cart.CartItems);
                await context.SaveChangesAsync();
            }

        }
    }
}
