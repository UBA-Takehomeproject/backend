using BlogPlatform.Application.DTOs;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Application.Extentions
{
    public static class AuthExtension
    {
        public static UserDto ToUserDto(this User user)
        {
            if (user == null) return null;

            return new UserDto
            {
                objectId = (Guid)user.ObjectId,
                fname = user.FirstName,
                lname = user.LastName,
                email = user.Email,

                photoURL = user.PhotoURL,
                authorsInfo = user.AuthorsInfo.ToDto(), 
                role = user.role.ToString(),

                // Add other properties as needed
            };
        }

        public static AuthResultDto ToAuthResultDto(this User user, string accessToken, string refreshToken)
        {
            return new AuthResultDto
            {
                user = user.ToUserDto(),
                accessToken = accessToken,
                refreshToken = refreshToken,
                success = true,
                errors = Array.Empty<string>() // No errors by default
            };
        }
    }
}