using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Domain.Entities
{
    public class Author
    {
        [Key, ForeignKey("User")] // Shared PK and FK
        public Guid ObjectId { get; set; }

        public User User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoURL { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    }
}
