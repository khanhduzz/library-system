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
    public class RolesControllerTests
    {
        private RolesController _controller;
        private LibrarySystemContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibrarySystemContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new LibrarySystemContext(options);
            _context.Database.EnsureDeleted();
            _controller = new RolesController(_context);
        }

        [Test]
        public async Task Index_ReturnsViewWithListOfRoles()
        {
            // Arrange
            var roles = new List<Role> { new Role { Id = 1, UserRole = Enums.UserRole.Admin } };
            _context.Role.AddRange(roles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.IsInstanceOf<List<Role>>(viewResult.Model);
            ClassicAssert.AreEqual(1, ((List<Role>)viewResult.Model).Count);
        }

        [Test]
        public async Task Details_ValidId_ReturnsViewWithRole()
        {
            // Arrange
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Details(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(role, viewResult.Model);
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
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };

            // Act
            var result = await _controller.Create(role);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var role = new Role { Id = 1 };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(role);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(role, viewResult.Model);
        }

        [Test]
        public async Task Edit_Get_ValidId_ReturnsViewWithRole()
        {
            // Arrange
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(role, viewResult.Model);
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
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(1, role);

            // Assert
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var role = new Role { Id = 1 };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(1, role);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(role, viewResult.Model);
        }

        [Test]
        public async Task Delete_Get_ValidId_ReturnsViewWithRole()
        {
            // Arrange
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(role, viewResult.Model);
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
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
            _context.Role.Add(role);
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