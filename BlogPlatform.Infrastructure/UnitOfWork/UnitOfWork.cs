using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Interfaces;
using BlogPlatform.Infrastructure.Persistence;
using BlogPlatform.Infrastructure.Repositories;

namespace BlogPlatform.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Blog>? _blogs;
        private IRepository<BlogPost>? _blogPosts;
        private IRepository<Author>? _authors;
        private IRepository<User>? _users;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Blog> Blogs => _blogs ??= new Repository<Blog>(_context);
        public IRepository<BlogPost> BlogPosts => _blogPosts ??= new Repository<BlogPost>(_context);
        public IRepository<Author> Authors => _authors ??= new Repository<Author>(_context);
        public IRepository<User> Users => _users ??= new Repository<User>(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
