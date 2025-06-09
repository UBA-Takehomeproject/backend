using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Domain.Entities
{
    public class BlogPost
    {
         [Key]
        public Guid ObjectId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string[] Category { get; set; }

        // Foreign key to Author
        public Guid AuthorsInfoObjectId { get; set; }
        public Author? AuthorsInfo { get; set; }
        public string CoverImage { get; set; }
        public string Href { get; set; }

        // Foreign key to Author
        public Guid BlogObjectId { get; set; }

        public string? Description { get; set; }

        // [ForeignKey("BlogObjectId")]
        public Blog Blog { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
       
    }
}