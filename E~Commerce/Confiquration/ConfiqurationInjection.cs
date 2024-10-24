using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Caching;
using Services.EmailServices;
using Services.HandleResponse;
using Services.Helper;
using Services.MappingProfile;
using Services.Services.AuthServices;
using Services.Services.BrandServices;
using Services.Services.CartServices;
using Services.Services.CategoryServices;
using Services.Services.OrderServices;
using Services.Services.Paymentservices;
using Services.Services.ProductServices;
using Services.Services.ProductTypeServices;
using Services.Services.RateServices;
using Services.Services.RoleServices;
using Services.Services.UserServices;
using Services.Services.UserServices.UserServices;
using Services.Services.WishListServices;
using StackExchange.Redis;
using System.Data.SqlTypes;

namespace E_Commerce.Confiquration
{
    public static class ConfiqurationInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork,UintOfWork>();
            services.AddTransient<IBrandServices,BrandServices>();
            services.AddTransient<IAuthServices,AuthServices>();
            services.AddTransient<IRoleServices,RoleServices>();
            services.AddTransient<IProductServices,ProductServices>();
            services.AddTransient<ITypeServices,TypeServices>();
            services.AddTransient<ICategoryServices,CategoryServices>();
            services.AddTransient<ICartServices,CartServices>();
            services.AddTransient<IOrderServices,OrderServices>();
            services.AddTransient<IWishListServices, WishListServices>();
            services.AddTransient<ICachServices, CachServices>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddTransient<IRateServices, RateServices>();
            services.AddTransient<IUserServices, UserService >();
            services.AddTransient<IPaymentServices, PaymentServices>();

            #region mapping
            services.AddAutoMapper(typeof(Brandprofile));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(TypeProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(CartProfile));
            services.AddAutoMapper(typeof(Orderprofile));
            services.AddAutoMapper(typeof(WishListProfile));
            services.AddAutoMapper(typeof(Userprofile));

            #endregion
            
            services.Configure<ApiBehaviorOptions>(c =>
            {
                c.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                    .Where(c => c.Value.Errors.Count > 0)
                    .SelectMany(c => c.Value.Errors)
                    .Select(c => c.ErrorMessage)
                    .ToList();

                    var response = new ValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }
    }
}
