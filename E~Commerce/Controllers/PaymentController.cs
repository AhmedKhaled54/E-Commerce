using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.HandleResponse;
using Services.Services.Paymentservices;
using Stripe;
using System.Drawing;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly string WebSecret;
        private readonly IPaymentServices services;
        public PaymentController(IPaymentServices services,IConfiguration configuration)
        {
            WebSecret = configuration["Stripe:WebhookSecret"];
            this.services = services;
           
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(int OrderId)
        {
            var payment = await services.CreatePayment(OrderId);
            if (!payment.IsSucceeded)
                return BadRequest(new ApiResponse(payment.Status, payment.Message));
            return Ok(payment.Message);
        }

        [HttpPost]
        public async Task<IActionResult> WebHock()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], WebSecret);

                PaymentIntent intent;

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    //var paymentintent = stripeEvent.Data.Object as PaymentIntent;

                    intent = (PaymentIntent)stripeEvent.Data.Object;

                    //update pamentintent in database 

                    await services.HandlePaymentSuccedded(intent.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    intent = (PaymentIntent)stripeEvent.Data.Object;

                    await services.HandlePaymentFaild(intent.Id);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }





    }
}
