using INFORCE_.NET_TASK.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public new DbSet<User> Users { get; set; }
    public DbSet<ShortUrl> ShortUrls { get; set; }
    public DbSet<About> Abouts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        builder.Entity<ShortUrl>()
            .HasIndex(o => o.OriginalUrl)
            .IsUnique();

        builder.Entity<ShortUrl>()
            .HasIndex(s => s.ShortCode)
            .IsUnique();

        var admin1Id = Guid.NewGuid();
        var admin2Id = Guid.NewGuid();
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        builder.Entity<User>().HasData
        (
            new User
            {
                Id = admin1Id,
                Username = "admin1",
                PasswordHash = GetPasswordHash("adminpass1"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = admin2Id,
                Username = "admin2",
                PasswordHash = GetPasswordHash("adminpass2"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = user1Id,
                Username = "user1",
                PasswordHash = GetPasswordHash("userpass1"),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = user2Id,
                Username = "user2",
                PasswordHash = GetPasswordHash("userpass2"),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            }
        );

        builder.Entity<About>().HasData
    (
        new About
        {
            Id = 1,
            Title = "URL Shortener Algorithm",
            Description = "Lorem ipsum",
            LastUpdated = DateTime.UtcNow,
            UpdatedByUserId = admin1Id
        }
    );
    }

        private static string GetPasswordHash(string password)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password)));
        }
}

