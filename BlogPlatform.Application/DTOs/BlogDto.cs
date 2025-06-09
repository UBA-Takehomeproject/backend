using System;

namespace BlogPlatform.Application.DTOs
{
    public class BlogDto
    {
        public Guid objectId { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public AuthorDto? authorsInfo { get; set; }

        public string description { get; set; }
        public ICollection<BlogPostDto>? blogPosts { get; set; }
        public Guid authorsObjectId { get; set; }
        public string coverImage { get; set; }
        public string? href { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }


}