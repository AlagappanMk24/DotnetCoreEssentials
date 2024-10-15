using JwtAuthDB.Dtos;
using JwtAuthDB.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace JwtAuthDB.Services.Interface
{ 
    public interface IAuthService
    {
        User AddUser(User user);
        string Login(LoginRequestDto loginRequestDto);
    }
}

