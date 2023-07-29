using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCorePress.Models;

namespace NetCorePress.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Post> Posts => Set<Post>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
            .HasIndex(p => p.Title)
            .IsUnique();

            builder.Entity<Post>()
            .HasOne<ApplicationUser>(p => p.User)
            .WithMany(a => a.Posts)
            .HasForeignKey(p => p.UserId);

            builder.Entity<Comment>()
            .HasOne<Post>(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId);

            builder.Entity<Comment>()
            .HasOne<ApplicationUser>(c => c.User)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.UserId);
        }
    }
}