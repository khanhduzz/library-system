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

            //modelBuilder.Entity<BorrowedBook>()
            //    .HasKey(bb => new { bb.Id });

            //modelBuilder.Entity<BorrowedBook>()
            //    .HasOne(bb => bb.User)
            //    .WithMany(u => u.BorrowedBooks)
            //    .HasForeignKey(bb => bb.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<BorrowedBook>()
            //    .HasOne(bb => bb.Book)
            //    .WithMany(b => b.BorrowedBooks)
            //    .HasForeignKey(bb => bb.BookId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
