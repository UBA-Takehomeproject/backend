using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Domain.Entities
{
    public class Blog
    {
        [Key]
        public Guid ObjectId { get; set; } // Primary key
        public string Title { get; set; }
        public string Date { get; set; }

        // Foreign key to Author
        public Guid AuthorsInfoObjectId { get; set; }

        public Author? AuthorsInfo { get; set; }
        public string? Description { get; set; }

        public string CoverImage { get; set; }
        public string Href { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    }
}
