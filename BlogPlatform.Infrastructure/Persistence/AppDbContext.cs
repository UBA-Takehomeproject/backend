using Microsoft.EntityFrameworkCore;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>()
            .HasOne(b => b.AuthorsInfo)
            .WithMany(a => a.Blogs)
            .HasForeignKey(b => b.AuthorsInfoObjectId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BlogPost>()
            .HasOne(bp => bp.AuthorsInfo)
            .WithMany(a => a.BlogPosts)
            .HasForeignKey(bp => bp.AuthorsInfoObjectId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
            .HasOne(u => u.AuthorsInfo)
            .WithOne(a => a.User)
            .HasForeignKey<Author>(a => a.ObjectId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: delete author when user is deleted

        }
    }
}
