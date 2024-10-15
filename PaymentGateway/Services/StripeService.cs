using Microsoft.Extensions.Options;
using PaymentGateway.Models;
using PaymentGateway.Services.Interface;
using PaymentGateway.Settings;
using Stripe;

namespace PaymentGateway.Services
{
    public class StripeService(IOptions<StripeSettings> stripeSettings) : IStripeService
    {
        private readonly StripeSettings _stripeSettings = stripeSettings.Value;

        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentRequestDto)
        {
            try
            {
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = (long)(paymentRequestDto.Amount * 100), // Amount in cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CreateAsync(createOptions);

                var confirmOptions = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = paymentRequestDto.PaymentMethodId
                };

                var confirmedPaymentIntent = await paymentIntentService.ConfirmAsync(paymentIntent.Id, confirmOptions);

                return new PaymentResponseDto
                {
                    ClientSecret = confirmedPaymentIntent.ClientSecret,
                    Status = confirmedPaymentIntent.Status
                };
            }
            catch (System.Exception ex)
            {
                throw new PaymentException("An error occurred while processing payment", ex);
            }
        }

        public async Task<PaymentStatusDto> CheckPaymentStatusAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);

                return new PaymentStatusDto
                {
                    Id = paymentIntent.Id,
                    Status = paymentIntent.Status,
                    AmountReceived = paymentIntent.AmountReceived,
                    Currency = paymentIntent.Currency
                };
            }
            catch (System.Exception ex)
            {
                throw new PaymentException("An error occurred while checking payment status", ex);
            }
        }
    }
}
