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
using LibrarySystem.Enums;

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
        public async Task<IActionResult> Index(string? searchString, int page = 1, int pageSize = 2)
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
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
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

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Name");
            ViewData["Genres"] = new MultiSelectList(_context.Genre, "Id", "Name"); // <-- new for genres
            ViewData["ConditionList"] = new SelectList(Enum.GetValues(typeof(BookCondition)));
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,AuthorId,ISBN,PublishDate,IsAvailable,Condition")] Book book, IFormFile? ImageFile, List<int>? selectedGenres)
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
            ViewData["ConditionList"] = new SelectList(Enum.GetValues(typeof(BookCondition)), book?.Condition);
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
            ViewData["ConditionList"] = new SelectList(Enum.GetValues(typeof(BookCondition)), book.Condition);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AuthorId,ISBN,PublishDate,IsAvailable,Condition")] Book updatedBook, IFormFile? ImageFile, List<int>? selectedGenres)
        {
            if (id != updatedBook.Id)
            {
                return NotFound();
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
                    book.IsAvailable = updatedBook.IsAvailable;
                    book.Condition = updatedBook.Condition;

                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageFile.CopyToAsync(memoryStream);
                            book.Image = memoryStream.ToArray();
                        }
                    }

                    // Update genres
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
            ViewData["ConditionList"] = new SelectList(Enum.GetValues(typeof(BookCondition)), updatedBook.Condition);
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
    }
}
