using GameBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBlog.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasKey(x => x.PostId);

            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    PostId = 1,
                    Title = "Test Title",
                    Content = "This is a post",
                    CreatedAt = DateTime.Now,
                    AuthorId = 1,
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "admin",
                });
        }

    }
}