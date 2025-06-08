using BlogPlatform.Application.DTOs;
using BlogPlatform.Application.Interfaces;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BlogPlatform.Application.Extentions;

namespace BlogPlatform.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<AuthResultDto> RegisterAsync(string firstName, string lastName, string password, string email)
        {
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).FirstOrDefault();
            if (existingUser != null)
                throw new Exception("User already exists");

            var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,
                role = Role.USER
            };

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, $"{newUser.FirstName} {newUser.LastName}"),
                new Claim(ClaimTypes.Email, newUser.Email),
                new Claim(ClaimTypes.Role, newUser.role.ToString())
            };

            var token = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            return newUser.ToAuthResultDto(token, refreshToken); ;
        }

        public async Task<AuthResultDto> LoginAsync(string email, string password)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).FirstOrDefault();
            var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            if (user?.PasswordHash != passwordHash)
            {
                return new AuthResultDto { success = false, errors = new[] { "Invalid credentials." } };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.role.ToString())
            };
            var token = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            return user.ToAuthResultDto(
                token,
                refreshToken
            );
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
