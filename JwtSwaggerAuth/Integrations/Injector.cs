using JwtSwaggerAuth.Services;
using JwtSwaggerAuth.Services.Interfaces;

namespace JwtSwaggerAuth.Integrations
{
    public static class Injector
    {
        public static void RegisterServices(this IServiceCollection services)
        {
           services.AddTransient<IJwtService, JwtService>();    
        }
    }
}
