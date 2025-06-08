namespace BlogPlatform.Application.DTOs
{
    public class AuthorDto
    {
        public Guid objectId { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string otherName { get; set; }
        public string bio { get; set; }
        public string photoURL { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}