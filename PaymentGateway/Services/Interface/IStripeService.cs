using PaymentGateway.Models;

namespace PaymentGateway.Services.Interface
{
    public interface IStripeService
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentRequestDto);
        Task<PaymentStatusDto> CheckPaymentStatusAsync(string paymentIntentId);
    }
}
