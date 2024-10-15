namespace PaymentGateway.Models
{
    public class PaymentStatusDto
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public long AmountReceived { get; set; }
        public string Currency { get; set; }
    }
}
