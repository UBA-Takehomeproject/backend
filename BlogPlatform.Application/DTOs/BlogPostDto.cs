using System;

namespace BlogPlatform.Application.DTOs
{
    public class BlogPostDto
    {
        public Guid objectId { get; set; }
        public string title { get; set; }
        public DateTime date { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public string[] category { get; set; }
        public AuthorDto? authorsInfo { get; set; }
        public Guid authorsObjectId { get; set; }
        public string coverImage { get; set; }
        public string? href { get; set; }
        public BlogDto? blog { get; set; }
        public Guid blogObjectId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }

}