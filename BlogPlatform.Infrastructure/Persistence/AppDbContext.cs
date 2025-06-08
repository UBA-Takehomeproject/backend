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

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>()
            .HasOne(b => b.AuthorsInfo)
            .WithMany(a => a.Blogs)
            .HasForeignKey(b => b.AuthorsInfoObjectId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BlogPost>()
            .HasOne(bp => bp.AuthorsInfo)
            .WithMany(a => a.BlogPosts)
            .HasForeignKey(bp => bp.AuthorsInfoObjectId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
            .HasOne(u => u.AuthorsInfo)
            .WithOne() // No reverse navigation property in Author
            .HasForeignKey<User>(u => u.ObjectId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
