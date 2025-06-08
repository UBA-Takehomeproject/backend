using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Application.DTOs
{
    public class UserDto
    {
        public Guid objectId { get; set; }
        public string email { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string photoURL { get; set; }
        public AuthorDto? authorsInfo { get; set; }
        public Role role { get; set; }  
    }

}