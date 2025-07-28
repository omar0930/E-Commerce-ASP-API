using Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.Cart.Dtos;
using Store.Services.Services.Payment;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        const string endpointSecret = "whsec_085d2203550c33b44431ba399189449dfc926a42e3573aa89c35b014df7c354d";
        private ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateOrUpdatePaymentIntent(CartDto cart)
        {
            return Ok(await paymentService.CreateOrUpdatePaymentIntent(cart));
        }

        // This is your Stripe CLI webhook secret for testing your endpoint locally.

        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                PaymentIntent paymentIntent;
                // Handle the event
                if (stripeEvent.Type == EventTypes.PaymentIntentCanceled)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("PaymentIntent {PaymentIntentId} canceled", paymentIntent.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentCreated)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("PaymentIntent {PaymentIntentId} created", paymentIntent.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentProcessing)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("PaymentIntent {PaymentIntentId} processing", paymentIntent.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    await paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("PaymentIntent {PaymentIntentId} succeeded", paymentIntent.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentMethodUpdated)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    await paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("PaymentIntent {PaymentIntentId} updated", paymentIntent.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
