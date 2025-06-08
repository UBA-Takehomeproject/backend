using System.Security.Claims;
using System.Threading.Tasks;
using BlogPlatform.Application.DTOs;
using BlogPlatform.Application.Extentions;
using BlogPlatform.Application.Interfaces;
using BlogPlatform.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("signup-jwt")]
        public IActionResult Signup([FromBody] SignupDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid user data.");
            }

            // Dummy check — replace with real user registration logic
            AuthResultDto? registeredUser = _authService.RegisterAsync(user.fname, user.lname, user.password, user.email).Result;
            if (registeredUser != null)
            {
                return Ok(registeredUser);
            }

            return BadRequest("User registration failed.");
        }

        [HttpPost("login-jwt")]
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

        /// <summary>
        /// Refreshes the JWT access and refresh tokens using the provided refresh token from cookie or Authorization header.
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // 1. Get refresh token from cookie or Authorization header
            string? refreshToken = null;
            bool jwtRequest = false;

            if (Request.Cookies.ContainsKey("refresh_token"))
            {
                refreshToken = Request.Cookies["refresh_token"];
            }
            else if (Request.Headers.ContainsKey("Authorization"))
            {
                jwtRequest = true;
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    refreshToken = authHeader.Substring("Bearer ".Length).Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("No refresh token");

            // 2. Validate & rotate token
            var result = await _authService.RefreshAsync(refreshToken);
            if (result == null || !result.success)
                return Unauthorized("Invalid refresh token");

            // 3. Send new tokens as HttpOnly cookies (only if not null)
            if (!string.IsNullOrEmpty(result.accessToken))
            {
                Response.Cookies.Append("access_token", result.accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });
            }

            if (!string.IsNullOrEmpty(result.refreshToken))
            {
                Response.Cookies.Append("refresh_token", result.refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }
            if (jwtRequest)
            {
                // If this was a JWT request, return the new tokens in the response
                return Ok(new
                {
                    accessToken = result.accessToken,
                    refreshToken = result.refreshToken
                });
            }
            return Ok(new { message = "Token refreshed" });
        }



        [HttpGet("me-jwt")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));

            if (user == null) return Unauthorized();

            return Ok(user.ToUserDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login2([FromBody] LoginDto login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.password))
                return BadRequest("Invalid login request.");

            var user = await _authService.LoginAsync(login.email, login.password);

            if (user == null || !user.success)
                return Unauthorized("Invalid credentials.");

            // Set HttpOnly cookies
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only send over HTTPS
                SameSite = SameSiteMode.None, // Important for cross-origin frontend
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("access_token", user.accessToken, accessTokenOptions);
            Response.Cookies.Append("refresh_token", user.refreshToken, refreshTokenOptions);

            // Return user info only — no tokens
            return Ok(user.user);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup2([FromBody] SignupDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
                return BadRequest("Invalid user data.");

            AuthResultDto? registeredUser = await _authService.RegisterAsync(user.fname, user.lname, user.password, user.email);

            if (registeredUser == null || !registeredUser.success)
                return BadRequest("User registration failed.");

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("access_token", registeredUser.accessToken, accessTokenOptions);
            Response.Cookies.Append("refresh_token", registeredUser.refreshToken, refreshTokenOptions);

            return Ok(registeredUser.user);
        }

        /// <summary>
        /// Logs out the current user by invalidating the refresh token and clearing authentication cookies.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.LogoutAsync(refreshToken);
            }

            //  Clear the cookies
            Response.Cookies.Delete("access_token", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("me")]
        public async Task<IActionResult> CheckAuth()
        {
            // Try to get user id from access_token cookie
            var accessToken = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(accessToken))
            {
                return Ok(new { isAuthenticated = false });
            }

            // Validate and extract user id from JWT
            var userId = _authService.GetUserIdFromToken(accessToken);
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(new { isAuthenticated = false });
            }

            var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId),u=>u.AuthorsInfo);
            if (user == null)
            {
                return Ok(new { isAuthenticated = false });
            }

            return Ok(new
            {
                isAuthenticated = true,
                user = user.ToUserDto()
            });
        }

    }



}
