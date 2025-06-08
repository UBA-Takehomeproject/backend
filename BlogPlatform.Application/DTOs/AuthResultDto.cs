
using BlogPlatform.Application.DTOs; // Adjust this namespace if UserDto is in a different namespace

namespace BlogPlatform.Application.DTOs
{
    public class AuthResultDto
    {
        public UserDto user { get; set; }
        public string? accessToken { get; set; }
        public string? refreshToken { get; set; }
        public bool success { get; set; }
        public string[] errors { get; set; } = [];
    }
}