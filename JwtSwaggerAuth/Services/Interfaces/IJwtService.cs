using JwtSwaggerAuth.Models;

namespace JwtSwaggerAuth.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtTokenAsync(LoginDto loginDto);
    }
}
