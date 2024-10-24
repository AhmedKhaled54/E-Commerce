using Azure.Core.GeoJson;
using Core.Data;
using Core.Entity;
using Core.Entity.OrderEntity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Dtos.Response;
using Services.HandleResponse;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Paymentservices
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IConfiguration config;
        private readonly IUnitOfWork unitOf;
        private readonly UserManager<User> userManager;
        private readonly AppDBContext context;

        public PaymentServices(
            IConfiguration config,
            IUnitOfWork unitOf,
            UserManager<User> userManager,
            AppDBContext context
            )
        {
            this.config = config;
            this.unitOf = unitOf;
            this.userManager = userManager;
            this.context = context;
        }
        public async Task<ResponseDto> CreatePayment(int OrdersId)
        {
            var response = new ResponseDto();

            StripeConfiguration.ApiKey = config["Stripe:Secretkey"];
            var order = await unitOf.Repository<Order>().FindAsync(c => c.Id == OrdersId);
            if (order is null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Invalid Order Number !"
                };
            }
            var services = new PaymentIntentService();
            PaymentIntent intent;

            var Option = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.Total * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            intent = await services.CreateAsync(Option);
            var Payment = new Payment
            {
                OrderId = OrdersId,
                CreatedAt = DateTime.UtcNow,
                Amount = order.Total,
                PaymentIntentId = intent.Id,
                Currency = intent.Currency,
                PaymentStstus = "Pending",
                Method = Option.PaymentMethod,
            };

            if (Payment != null)
            {
                await unitOf.Repository<Payment>().AddAsync(Payment);
            }
            unitOf.Repository<Order>().Delete(order);
            await unitOf.Complete();

            return new ResponseDto
            {
                Status = 200,
                Message = $"Client Secret :{intent.ClientSecret}",
                IsSucceeded = true
            };

        }
        public async Task HandlePaymentFaild(string  PaymentIntentId)
        {
            var payment = await unitOf.Repository<Payment>().FindAsync(c => c.PaymentIntentId==PaymentIntentId);
            if (payment == null)
                return;
            payment.PaymentStstus = "PaymentFaild";
            unitOf.Repository<Payment>().Update(payment);
            await unitOf.Complete();
        }

        public async  Task HandlePaymentSuccedded(string  PaymentIntentId)
        {
            var payment = await unitOf.Repository<Payment>().FindAsync(c => c.PaymentIntentId == PaymentIntentId);
            if (payment == null)
                return;
            payment.PaymentStstus = "PaymentSuccedded";
            unitOf.Repository<Payment>().Update(payment);
            await unitOf.Complete();
        }
    }
}
