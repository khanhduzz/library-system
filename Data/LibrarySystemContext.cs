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
        public LibrarySystemContext (DbContextOptions<LibrarySystemContext> options)
            : base(options)
        {
        }

        public DbSet<LibrarySystem.Models.Book> Book { get; set; } = default!;
        public DbSet<LibrarySystem.Models.Author> Author { get; set; } = default!;
        public DbSet<LibrarySystem.Models.User> User { get; set; } = default!;
        public DbSet<LibrarySystem.Models.Role> Role { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<BookGenre> BookGenre { get; set; } = default!; // <-- ADD THIS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId }); // <-- Define composite primary key

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);
        }
    }
}
