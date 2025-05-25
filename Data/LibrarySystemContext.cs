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

        public void SeedBookImages()
        {
            var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "books");

            var bookImageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "The Da Vinci Code", "the-da-vinci-code.jpg" },
                { "Angels & Demons", "angels-and-demons.jpg" },
                { "The Silence of the Lambs", "the-silence-of-the-lambs.jpg" },
                { "Red Dragon", "red-dragon.jpg" },
                { "Murder on the Orient Express", "murder-on-the-orient-express.jpg" },
                { "And Then There Were None", "and-then-there-were-none.jpg" },
                { "Harry Potter and the Sorcerer's Stone", "harry-potter-and-the-sorcerers-stone.jpg" },
                { "Harry Potter and the Chamber of Secrets", "harry-potter-and-the-chamber-of-secrets.jpg" },
                { "The Shining", "the-shining.jpg" },
                { "It", "it.jpg" },
                { "A Game of Thrones", "a-game-of-thrones.jpg" },
                { "A Clash of Kings", "a-clash-of-kings.jpg" },
                { "The Fellowship of the Ring", "the-fellowship-of-the-ring.jpg" },
                { "The Two Towers", "the-two-towers.jpg" },
                { "Kafka on the Shore", "kafka-on-the-shore.jpg" },
                { "Norwegian Wood", "norwegian-wood.jpg" },
                { "Pride and Prejudice", "pride-and-prejudice.jpg" },
                { "Sense and Sensibility", "sense-and-sensibility.jpg" },
                { "The Old Man and the Sea", "the-old-man-and-the-sea.jpg" },
                { "A Farewell to Arms", "a-farewell-to-arms.jpg" }
            };

            var books = Book.ToList();
            bool updated = false;

            foreach (var book in books)
            {
                if (book.Image != null)
                    continue;

                if (bookImageMap.TryGetValue(book.Title, out var fileName))
                {
                    var imagePath = Path.Combine(imageDir, fileName);

                    if (File.Exists(imagePath))
                    {
                        book.Image = File.ReadAllBytes(imagePath);
                        updated = true;
                    }
                    else
                    {
                        Console.WriteLine($"Image not found for book: {book.Title} at {imagePath}");
                    }
                }
                else
                {
                    Console.WriteLine($"No image mapping found for book: {book.Title}");
                }
            }

            if (updated)
            {
                SaveChanges();
            }
        }
    }
}
