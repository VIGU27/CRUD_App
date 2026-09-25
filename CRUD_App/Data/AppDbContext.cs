using CRUD_App.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_App.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Content> Contents => Set<Content>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Content>(entity =>
        {
            entity.Property(c => c.Status).HasConversion<string>();
            entity.Property(c => c.Language).HasConversion<string>();

            entity.HasOne(c => c.Author)
                .WithMany(u => u.Contents)
                .HasForeignKey(c => c.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Article)
                .WithMany(a => a.Contents)
                .HasForeignKey(c => c.ArticleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(a => a.Status).HasConversion<string>();
        });

        //data seeded for user table
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "Ramesh", CreatedAt = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, Username = "Manoj", CreatedAt = new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
