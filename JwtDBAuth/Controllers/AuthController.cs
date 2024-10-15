using JwtAuthDB.Dtos;
using JwtAuthDB.Entities;
using JwtAuthDB.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        [HttpPost("signup")]
        public ActionResult<User> Signup([FromBody] User user)
        {
            try
            {
                var addedUser = _authService.AddUser(user);
                return CreatedAtAction(nameof(Signup), new { id = addedUser.Id, name = addedUser.Name });
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "An error occurred while adding a user.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginRequestDto loginModel)
        {
            try
            {
                var result = _authService.Login(loginModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "An error occurred during login.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addUser")]
        public ActionResult<User> AddUser([FromBody] User user)
        {
            try
            {
                var addedUser = _authService.AddUser(user);
                return CreatedAtAction(nameof(AddUser), new { id = addedUser.Id }, addedUser);
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "An error occurred while adding a user.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
