namespace PaymentGateway.Services
{
    public class PaymentException : Exception
    {
        public PaymentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
