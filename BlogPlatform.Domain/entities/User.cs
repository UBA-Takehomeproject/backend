using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Domain.Entities
{
  public class User
  {
    [Key]
    public Guid ObjectId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhotoURL { get; set; }

    // Foreign key to Author
    public Guid? AuthorsInfoObjectId { get; set; }

    public Author? AuthorsInfo { get; set; }
    public Role role { get; set; }
    public string PasswordHash { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

  }
}


