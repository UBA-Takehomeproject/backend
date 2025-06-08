using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Blog> Blogs { get; }
        IRepository<BlogPost> BlogPosts { get; }
        IRepository<Author> Authors { get; }
        IRepository<User> Users { get; }
        Task<int> SaveChangesAsync();
    }
}
