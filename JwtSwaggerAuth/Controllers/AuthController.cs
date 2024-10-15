using JwtSwaggerAuth.Models;
using JwtSwaggerAuth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtSwaggerAuth.Controllers
{
    [ApiController]
    public class AuthController(IJwtService jwtService) : ControllerBase
    {
        private readonly IJwtService _jwtService = jwtService;

        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _jwtService.GenerateJwtTokenAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
