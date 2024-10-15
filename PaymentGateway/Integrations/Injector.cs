using PaymentGateway.Services;
using PaymentGateway.Services.Interface;

namespace PaymentGateway.Integrations
{
    public static class Injector
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IStripeService, StripeService>();
        }
    }
}
