using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CEM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "user" && request.Password == "password") // Kiểm tra username/password
            {
                var token = _tokenService.GenerateToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
