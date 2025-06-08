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
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Email == email, u => u.AuthorsInfo)).FirstOrDefault();
            if (existingUser != null)
                throw new Exception("User already exists");

            var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            var newUser = new User
            {
                ObjectId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,
                role = Role.USER
            };

            await _unitOfWork.Users.AddAsync(newUser);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,newUser.ObjectId.ToString()),
                new Claim(ClaimTypes.Name, $"{newUser.FirstName} {newUser.LastName}"),
                new Claim(ClaimTypes.Email, newUser.Email),
                new Claim(ClaimTypes.Role, newUser.role.ToString())
            };

            var token = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();

            //save refresh token to db
            await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = newUser.ObjectId,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            await _unitOfWork.SaveChangesAsync();

            return newUser.ToAuthResultDto(token, refreshToken); ;
        }

        public async Task<AuthResultDto> LoginAsync(string email, string password)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email, u => u.AuthorsInfo)).FirstOrDefault();
            var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            if (user?.PasswordHash != passwordHash)
            {
                return new AuthResultDto { success = false, errors = new[] { "Invalid credentials." } };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.ObjectId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.role.ToString())
            };
            var token = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();

            //save refresh token to db
            await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.ObjectId,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            await _unitOfWork.SaveChangesAsync();

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

        public async Task<AuthResultDto> RefreshAsync(string refreshToken)
        {
            var storedToken = (await _unitOfWork.RefreshTokens.FindAsync(rt => rt.Token == refreshToken)).FirstOrDefault();

            if (storedToken == null || !storedToken.IsActive)
                return new AuthResultDto { success = false, errors = new[] { "Invalid refresh token." } };

            var user = await _unitOfWork.Users.GetByIdAsync(storedToken.UserId);
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier,user.ObjectId.ToString()),
               new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role, user.role.ToString())
            };

            var newAccessToken = GenerateAccessToken(claims);
            var newRefreshToken = GenerateRefreshToken();

            storedToken.Revoked = DateTime.UtcNow;
            await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.ObjectId,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            await _unitOfWork.SaveChangesAsync();

            return user.ToAuthResultDto(newAccessToken, newRefreshToken);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var storedToken = (await _unitOfWork.RefreshTokens.FindAsync(rt => rt.Token == refreshToken)).FirstOrDefault();

            if (storedToken != null && storedToken.IsActive)
            {
                storedToken.Revoked = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public string GetUserIdFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "id");
            return userIdClaim?.Value ?? throw new Exception("User ID not found in token");
        }
    }
}
