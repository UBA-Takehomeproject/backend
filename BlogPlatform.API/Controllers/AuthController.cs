using BlogPlatform.Application.DTOs;
using BlogPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;


namespace BlogPlatform.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }
 
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid user data.");
            }

            // Dummy check — replace with real user registration logic
            AuthResultDto? registeredUser = _authService.RegisterAsync(user.fname,user.lname, user.password, user.email).Result;
            if (registeredUser != null)
            {
                return Ok(registeredUser);
            }

            return BadRequest("User registration failed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            // Dummy check — replace with real user validation
            AuthResultDto? user = await _authService.LoginAsync(login.email, login.password);
            if (user != null)
            {
                return Ok(user);
            }

            return Unauthorized("Invalid credentials.");
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
       
            return Unauthorized("Invalid refresh token.");
        }
    }

}
