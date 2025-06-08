using BlogPlatform.Domain.Entities;
using BlogPlatform.Application.DTOs;
using BlogPlatform.Application.Extentions;

namespace BlogPlatform.Application.Extensions
{
    public static class BlogExtensions
    {
        public static BlogDto ToDto(this Blog blog)
        {
            if (blog == null) return null;

            return new BlogDto
            {
                objectId = (Guid)blog.ObjectId,
                title = blog.Title,
                authorsInfo = blog.AuthorsInfo?.ToDto(),
                createdAt = (DateTime)blog.CreatedAt,
                updatedAt = (DateTime)blog.UpdatedAt,
                date = blog.Date,
                coverImage = blog.CoverImage,
                href = blog.Href
            };
        }
        public static List<BlogDto> ToDtoList(this IEnumerable<Blog> blog)
        {
            if (blog == null) return null;
            return blog.Select(a => a.ToDto()).ToList();
        }
        public static Blog ToEntity(this BlogDto blogDto)
        {
            if (blogDto == null) return null;

            return new Blog
            {
                ObjectId = blogDto.objectId,
                Title = blogDto.title,
                AuthorsInfo = blogDto.authorsInfo?.ToEntity(),
                CreatedAt = blogDto.createdAt,
                UpdatedAt = blogDto.updatedAt,
                Date = blogDto.date,
                CoverImage = blogDto.coverImage,
                Href = blogDto.href
            };
        }

        public static List<Blog> ToEntityList(this IEnumerable<BlogDto> blogDtos)
        {
            if (blogDtos == null) return null;
            return blogDtos.Select(b => b.ToEntity()).ToList();
        }
    }
}