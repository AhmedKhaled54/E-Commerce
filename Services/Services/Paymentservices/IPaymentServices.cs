using Services.Dtos.Response;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Paymentservices
{
    public  interface IPaymentServices
    {
        Task<ResponseDto>CreatePayment(int OrdersId);
        Task HandlePaymentSuccedded (string PaymentIntentId);
        Task HandlePaymentFaild (string PaymentIntentId);

    }
}
