using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using LibrarySystem.Helpers;
using System.Security.Claims;
using System.Net;

namespace LibrarySystem.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibrarySystemContext _context;

        public BooksController(LibrarySystemContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string? searchString, int page = 1, int pageSize = 5)
        {
            var booksQuery = _context.Book.Include(b => b.Author).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchString) || b.Author.Name.Contains(searchString));
            }

            var paginatedBooks = await PaginatedList<Book>.CreateAsync(booksQuery.OrderBy(b => b.Title), page, pageSize, searchString);

            return View(paginatedBooks);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Book
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.BookId == id);

            // Then query BorrowedBook with the userId
            //var borrowed;
            //if (inventory != null)
            //{
            //    borrowed = await _context.BorrowedBook
            //        .FirstOrDefaultAsync(b => b.InventoryId == inventory.Id && b.UserId == userId && b.ReturnDate == null);
            //}

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                //IsBorrowedByUser = borrowed != null
            };

            return View(viewModel);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name");
            ViewData["Genres"] = new MultiSelectList(_context.Genre, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,AuthorId,ISBN,PublishDate")] Book book, IFormFile? ImageFile, List<int>? selectedGenres)
        {
            if (await _context.Book.AnyAsync(b => b.ISBN == book.ISBN))
            {
                ModelState.AddModelError("ISBN", "ISBN already exists. Please enter a unique ISBN.");
            }

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(memoryStream);
                        book.Image = memoryStream.ToArray();
                    }
                }

                if (selectedGenres != null)
                {
                    book.BookGenres = selectedGenres.Select(g => new BookGenre { GenreId = g }).ToList();
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name", book?.AuthorId);
            ViewData["Genres"] = new MultiSelectList(_context.Genre, "Id", "Name", selectedGenres);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name", book.AuthorId);
            ViewData["Genres"] = new MultiSelectList(_context.Genre, "Id", "Name", book.BookGenres.Select(bg => bg.GenreId));
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AuthorId,ISBN,PublishDate")] Book updatedBook, IFormFile? ImageFile, List<int>? selectedGenres)
        {
            if (id != updatedBook.Id)
            {
                return NotFound();
            }

            if (await _context.Book.AnyAsync(b => b.ISBN == updatedBook.ISBN && b.Id != updatedBook.Id))
            {
                ModelState.AddModelError("ISBN", "ISBN already exists. Please enter a unique ISBN.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _context.Book
                        .Include(b => b.BookGenres)
                        .FirstOrDefaultAsync(b => b.Id == id);

                    if (book == null)
                    {
                        return NotFound();
                    }

                    book.Title = updatedBook.Title;
                    book.Description = updatedBook.Description;
                    book.AuthorId = updatedBook.AuthorId;
                    book.ISBN = updatedBook.ISBN;
                    book.PublishDate = updatedBook.PublishDate;

                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageFile.CopyToAsync(memoryStream);
                            book.Image = memoryStream.ToArray();
                        }
                    }

                    book.BookGenres.Clear();
                    if (selectedGenres != null)
                    {
                        foreach (var genreId in selectedGenres)
                        {
                            book.BookGenres.Add(new BookGenre { GenreId = genreId });
                        }
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(updatedBook.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name", updatedBook.AuthorId);
            ViewData["Genres"] = new MultiSelectList(_context.Genre, "Id", "Name", selectedGenres);
            return View(updatedBook);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Borrow(int bookId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var inventory = await _context.Inventory
                            .FirstOrDefaultAsync(i => i.BookId == bookId);

            // Then query BorrowedBook with the userId
            var borrowed = await _context.BorrowedBook
                .FirstOrDefaultAsync(b => b.InventoryId == inventory.Id && b.UserId == userId && b.ReturnDate == null);

            if (borrowed != null)
            {
                TempData["Error"] = "You have already borrowed this book and haven't returned it yet.";
                return RedirectToAction("Details", new { id = bookId });
            }

            var book = await _context.Book.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction("Details", new { id = bookId });
            }

            //var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.BookId == book.Id);
            if (inventory == null || inventory.AvailableCopies <= 0)
            {
                TempData["Error"] = "No copies available to borrow.";
                return RedirectToAction("Details", new { id = bookId });
            }

            await BorrowBookAsync(bookId);
            TempData["Success"] = "You have successfully borrowed the book!";
            return RedirectToAction("Details", new { id = bookId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Return(int bookId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var book = await _context.Book.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction("Details", new { id = bookId });
            }

            var inventory = await _context.Inventory
                            .FirstOrDefaultAsync(i => i.BookId == bookId);

            // Then query BorrowedBook with the userId
            var borrowed = await _context.BorrowedBook
                .FirstOrDefaultAsync(b => b.InventoryId == inventory.Id && b.UserId == userId && b.ReturnDate != null);

            if (borrowed == null)
            {
                TempData["Error"] = "This book was not borrowed or is already returned.";
                return RedirectToAction("Details", new { id = bookId });
            }

            await ReturnBookAsync(userId, book.Id);
            TempData["Success"] = "You have successfully returned the book!";
            return RedirectToAction("Details", new { id = bookId });
        }

        public async Task ReturnBookAsync(int userId, int bookId)
        {
            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.BookId == bookId);

            // Then query BorrowedBook with the userId
            var borrowed = await _context.BorrowedBook
                .FirstOrDefaultAsync(b => b.InventoryId == inventory.Id && b.UserId == userId && b.ReturnDate == null);

            if (borrowed == null)
            {
                throw new InvalidOperationException("This book was not borrowed or already returned.");
            }

            borrowed.ReturnDate = DateTime.UtcNow;

            if (inventory != null)
            {
                inventory.AvailableCopies += 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task BorrowBookAsync(int bookId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.BookId == bookId);

            if (inventory == null || inventory.AvailableCopies <= 0)
            {
                throw new InvalidOperationException("No copies available to borrow.");
            }

            var borrowedBook = new BorrowedBook
            {
                UserId = userId,
                InventoryId = inventory.Id,
                BorrowDate = DateTime.UtcNow,
                Status = "Borrowed"
            };

            inventory.AvailableCopies -= 1;

            _context.BorrowedBook.Add(borrowedBook);
            await _context.SaveChangesAsync();
        }
    }
}
