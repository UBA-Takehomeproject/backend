using System.Security.Claims;
using BlogPlatform.Application.DTOs;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(string firstName,string lastName, string password, string email);
        Task<AuthResultDto> LoginAsync(string email, string password);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();

    }
}