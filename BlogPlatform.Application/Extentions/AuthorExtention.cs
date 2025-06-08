using BlogPlatform.Domain.Entities;
using BlogPlatform.Application.DTOs;

namespace BlogPlatform.Application.Extentions
{
    public static class AuthorExtension
    {
        public static AuthorDto ToDto(this Author author)
        {
            if (author == null) return null;

            return new AuthorDto
            {
                objectId = (Guid)author.ObjectId,
                fName = author.FirstName,
                lName = author.LastName,
                otherName = author.OtherName,
                bio = author.Bio,
                photoURL = author.PhotoURL,
                createdAt = (DateTime)author.CreatedAt,
                updatedAt = (DateTime)author.UpdatedAt
            };
        }
        public static Author ToEntity(this AuthorDto authorDto)
        {
            if (authorDto == null) return null;

            return new Author
            {
                ObjectId = authorDto.objectId != Guid.Empty ? authorDto.objectId : Guid.NewGuid(),
                FirstName = authorDto.fName,
                LastName = authorDto.lName,
                OtherName = authorDto.otherName,
                Bio = authorDto.bio,
                PhotoURL = authorDto.photoURL,
                CreatedAt = authorDto.createdAt,
                UpdatedAt = authorDto.updatedAt
            };
        }

        public static List<AuthorDto> ToDtoList(this IEnumerable<Author> authors)
        {
            if (authors == null) return null;
            return authors.Select(a => a.ToDto()).ToList();
        }
    }
}