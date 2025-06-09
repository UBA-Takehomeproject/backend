using BlogPlatform.Domain.Entities;
using BlogPlatform.Application.DTOs;
using BlogPlatform.Application.Extensions;

namespace BlogPlatform.Application.Extentions
{
    public static class BlogPostExtention
    {
        public static BlogPostDto ToDto(this BlogPost blogPost)
        {
            if (blogPost == null) return null;

            return new BlogPostDto
            {
                objectId = blogPost.ObjectId,
                title = blogPost.Title,
                content = blogPost.Content,
                authorsInfo = blogPost.AuthorsInfo?.ToDto(),
                authorsObjectId = blogPost.AuthorsInfo?.ObjectId ?? blogPost.AuthorsInfoObjectId,
                description = blogPost.Description ?? "",
                date = blogPost.Date, // Format date as needed
                blog = blogPost.Blog?.ToDto(),
                category = blogPost.Category,
                coverImage = blogPost.CoverImage,
                href = blogPost.Href,
                blogObjectId = blogPost.BlogObjectId,
                createdAt = (DateTime)blogPost.CreatedAt,
                updatedAt = blogPost.UpdatedAt,
                
                // Add other properties as needed
            };
        }
          public static BlogPost ToEntity(this BlogPostDto blogDto)
        {
            if (blogDto == null) return null;

            return new BlogPost
            {
                ObjectId = blogDto.objectId,
                Title = blogDto.title,
                AuthorsInfo = blogDto.authorsInfo?.ToEntity(),
                CreatedAt = blogDto.createdAt,
                UpdatedAt = blogDto.updatedAt,
                Description = blogDto.description,
                AuthorsInfoObjectId = blogDto.authorsObjectId,
                BlogObjectId = blogDto.blogObjectId, // Assuming blog is optional
                CoverImage = blogDto.coverImage,
                Href = blogDto.href,
                Category = blogDto.category,
                Content = blogDto.content,
                Date =  blogDto.date,
            };
        }
        public static List<BlogPostDto> ToDtoList(this IEnumerable<BlogPost> blogPost)
        {
            if (blogPost == null) return new List<BlogPostDto>();
            return blogPost.Select(a => a.ToDto()).ToList();
        }

    }
}