using JwtAuthDB.Data;
using JwtAuthDB.Data.Repositories.Interface;
using JwtAuthDB.Dtos;
using JwtAuthDB.Entities;
using JwtAuthDB.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthDB.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;
        public User AddUser(User user)
        {
            // Check if the user already exists
            var existingUser = _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            return _userRepository.AddUser(user);
        }
        public string Login(LoginRequestDto loginRequestDto)
        {
            var user = _userRepository.GetUserByEmail(loginRequestDto.Email);
            if (user == null || user.Password != loginRequestDto.Password)
            {
                throw new Exception("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
