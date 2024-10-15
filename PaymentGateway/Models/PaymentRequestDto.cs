namespace PaymentGateway.Models
{
    public class PaymentRequestDto
    {
        public required decimal Amount { get; set; }  // Amount in USD
        public required string PaymentMethodId { get; set; }  // Payment method ID (e.g., 'pm_card_visa' for testing)
    }
}
