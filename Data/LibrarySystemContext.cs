using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;

namespace LibrarySystem.Data
{
    public class LibrarySystemContext : DbContext
    {
        public LibrarySystemContext(DbContextOptions<LibrarySystemContext> options)
            : base(options)
        {
        }

        public DbSet<LibrarySystem.Models.Book> Book { get; set; } = default!;
        public DbSet<LibrarySystem.Models.Author> Author { get; set; } = default!;
        public DbSet<LibrarySystem.Models.User> User { get; set; } = default!;
        public DbSet<LibrarySystem.Models.Inventory> Inventory { get; set; } = default!;
        public DbSet<LibrarySystem.Models.Role> Role { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<BookGenre> BookGenre { get; set; } = default!;
        public DbSet<BorrowedBook> BorrowedBook { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);
        }
        public void SeedAuthorImages()
        {
            var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "authors");

            var authorImageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Dan Brown", "dan-brown.jpg" },
                { "Thomas Harris", "thomas-harris.jpg" },
                { "Agatha Christie", "agatha-christie.jpg" },
                { "J.K. Rowling", "j-k-rowling.jpg" },
                { "Stephen King", "stephen-king.jpg" },
                { "George R.R. Martin", "george-rr-martin.jpg" },
                { "J.R.R. Tolkien", "jrrr-tolkien.jpg" },
                { "Haruki Murakami", "haruki-murakami.jpg" },
                { "Jane Austen", "jane-austen.jpg" },
                { "Ernest Hemingway", "ernest-hemingway.webp" }
            };

            var authors = Author.ToList();
            bool updated = false;

            foreach (var author in authors)
            {
                if (author.Image != null)
                    continue;

                if (authorImageMap.TryGetValue(author.Name, out var fileName))
                {
                    var imagePath = Path.Combine(imageDir, fileName);

                    if (File.Exists(imagePath))
                    {
                        author.Image = File.ReadAllBytes(imagePath);
                        updated = true;
                    }
                    else
                    {
                        Console.WriteLine($"Image not found for author: {author.Name} at {imagePath}");
                    }
                }
                else
                {
                    Console.WriteLine($"No image mapping found for author: {author.Name}");
                }
            }

            if (updated)
            {
                SaveChanges();
            }
        }

    }
}
