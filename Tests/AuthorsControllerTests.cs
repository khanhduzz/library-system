using LibrarySystem.Controllers;
using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Threading.Tasks;

namespace LibrarySystem.Tests
{
    [TestFixture]
    public class AuthorsControllerTests
    {
        private AuthorsController _controller;
        private LibrarySystemContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibrarySystemContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new LibrarySystemContext(options);
            _context.Database.EnsureDeleted();
            _controller = new AuthorsController(_context);
        }

        [Test]
        public async Task Index_ReturnsViewWithListOfAuthors()
        {
            // Arrange
            var authors = new List<Author> { new Author { Id = 1, Name = "Author1" } };
            _context.Author.AddRange(authors);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.IsInstanceOf<List<Author>>(viewResult.Model);
            ClassicAssert.AreEqual(1, ((List<Author>)viewResult.Model).Count);
        }

        [Test]
        public async Task Details_ValidId_ReturnsViewWithAuthor()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Details(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(author, viewResult.Model);
        }

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            // Act
            var result = _controller.Create();

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author1" };

            // Act
            var result = await _controller.Create(author, null);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var author = new Author { Id = 1 };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(author, null);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(author, viewResult.Model);
        }

        [Test]
        public async Task Edit_Get_ValidId_ReturnsViewWithAuthor()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(author, viewResult.Model);
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
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1, author, null);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var author = new Author { Id = 1 };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(1, author, null);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(author, viewResult.Model);
        }

        [Test]
        public async Task Delete_Get_ValidId_ReturnsViewWithAuthor()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(author, viewResult.Model);
        }

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
            var author = new Author { Id = 1, Name = "Author1" };
            _context.Author.Add(author);
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