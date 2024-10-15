using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using PaymentGateway.Services;
using PaymentGateway.Services.Interface;

namespace PaymentGateway.Controllers
{
    [ApiController]
    public class PaymentController(IStripeService paymentService) : ControllerBase
    {
        private readonly IStripeService _paymentService = paymentService;

        [HttpPost("api/payment/process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto paymentRequestDto)
        {
            try
            {
                var result = await _paymentService.ProcessPaymentAsync(paymentRequestDto);
                return Ok(result);
            }
            catch (PaymentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("api/check-payment-status/{paymentIntentId}")]
        public async Task<IActionResult> CheckPaymentStatus(string paymentIntentId)
        {
            try
            {
                var result = await _paymentService.CheckPaymentStatusAsync(paymentIntentId);
                return Ok(result);
            }
            catch (PaymentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //// Webhook to handle Stripe events (optional)
        //[HttpPost("api/payment/webhook")]
        //public async Task<IActionResult> StripeWebhook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _stripeSettings.SecretKey);

        //    if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //    {
        //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //        // Handle successful payment
        //    }

        //    return Ok();
        //}
    }
}
