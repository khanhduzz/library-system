using LibrarySystem.Controllers;
using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Threading.Tasks;

namespace LibrarySystem.Tests
{
    [TestFixture]
    public class BooksControllerTests
    {
        private BooksController _controller;
        private LibrarySystemContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibrarySystemContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new LibrarySystemContext(options);
            _context.Database.EnsureDeleted();
            _controller = new BooksController(_context);
        }

        //[Test]
        //public async Task Index_ReturnsViewWithListOfBooks()
        //{
        //    // Arrange
        //    var books = new List<Book> { new Book { Id = 1, Title = "Book1", AuthorId = 1 } };
        //    _context.Book.AddRange(books);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Index();

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.IsInstanceOf<List<Book>>(viewResult.Model);
        //    ClassicAssert.AreEqual(1, ((List<Book>)viewResult.Model).Count);
        //}

        //[Test]
        //public async Task Details_ValidId_ReturnsViewWithBook()
        //{
        //    // Arrange
        //    var book = new Book { Id = 1, Title = "Book1", AuthorId = 1 };
        //    _context.Book.Add(book);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Details(1);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.AreEqual(book, viewResult.Model);
        //}

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_Get_ReturnsViewWithSelectList()
        {
            // Arrange
            var authors = new List<Author> { new Author { Id = 1, Name = "Author1" } };
            _context.Author.AddRange(authors);
            _context.SaveChanges();

            // Act
            var result = _controller.Create();

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult.ViewData["AuthorId"]);
            ClassicAssert.IsInstanceOf<SelectList>(viewResult.ViewData["AuthorId"]);
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book1", AuthorId = 1 };
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Create(book, null);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var book = new Book { Id = 1, AuthorId = 1, Title = "title" };
            _controller.ModelState.AddModelError("Title", "Required");
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Create(book, null);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(book, viewResult.Model);
            ClassicAssert.IsNotNull(viewResult.ViewData["AuthorId"]);
        }

        [Test]
        public async Task Edit_Get_ValidId_ReturnsViewWithBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book1", AuthorId = 1 };
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Book.Add(book);
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(book, viewResult.Model);
            ClassicAssert.IsNotNull(viewResult.ViewData["AuthorId"]);
        }

        [Test]
        public async Task Edit_Get_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book1", AuthorId = 1 };
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Book.Add(book);
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1, book);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var book = new Book { Id = 1, AuthorId = 1, Title = "title" };
            _controller.ModelState.AddModelError("Title", "Required");
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1, book);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(book, viewResult.Model);
            ClassicAssert.IsNotNull(viewResult.ViewData["AuthorId"]);
        }

        //[Test]
        //public async Task Delete_Get_ValidId_ReturnsViewWithBook()
        //{
        //    // Arrange
        //    var book = new Book { Id = 1, Title = "Book1", AuthorId = 1 };
        //    _context.Book.Add(book);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Delete(1);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.AreEqual(book, viewResult.Model);
        //}

        [Test]
        public async Task Delete_Get_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Delete(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_ValidId_RedirectsToIndex()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book1", AuthorId = 1 };
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }
    }
} 