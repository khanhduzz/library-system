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
    public class UsersControllerTests
    {
        private UsersController _controller;
        private LibrarySystemContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibrarySystemContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new LibrarySystemContext(options);
            _context.Database.EnsureDeleted();
            _controller = new UsersController(_context);
        }

        //[Test]
        //public async Task Index_ReturnsViewWithListOfUsers()
        //{
        //    // Arrange
        //    var users = new List<User> { new User { Id = 1, UserName = "User1" } };
        //    _context.User.AddRange(users);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Index();

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.IsInstanceOf<List<User>>(viewResult.Model);
        //    ClassicAssert.AreEqual(1, ((List<User>)viewResult.Model).Count);
        //}

        //[Test]
        //public async Task Details_ValidId_ReturnsViewWithUser()
        //{
        //    // Arrange
        //    var user = new User { Id = 1, UserName = "User1" };
        //    _context.User.Add(user);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Details(1);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.AreEqual(user, viewResult.Model);
        //}

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        //[Test]
        //public void Create_Get_ReturnsViewWithSelectList()
        //{
        //    // Arrange
        //    var roles = new List<Role> { new Role { Id = 1, UserRole = Enums.UserRole.Admin } };
        //    _context.Role.AddRange(roles);
        //    _context.SaveChanges();

        //    // Act
        //    var result = _controller.Create();

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.IsNotNull(viewResult.ViewData["Roles"]);
        //    ClassicAssert.IsInstanceOf<MultiSelectList>(viewResult.ViewData["Roles"]);
        //}

        //[Test]
        //public async Task Create_Post_ValidModel_RedirectsToIndex()
        //{
        //    // Arrange
        //    var user = new User { Id = 1, UserName = "User1", Password = "password" };
        //    var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
        //    _context.Role.Add(role);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Create(user, new List<int> { 1 });

        //    // Assert
        //    ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
        //    var redirectResult = result as RedirectToActionResult;
        //    ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        //}

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var user = new User { Id = 1 };
            _controller.ModelState.AddModelError("UserName", "Required");
            var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Create(user, new List<int> { 1 });

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.AreEqual(user, viewResult.Model);
            ClassicAssert.IsNotNull(viewResult.ViewData["Roles"]);
        }

        //[Test]
        //public async Task Edit_Get_ValidId_ReturnsViewWithUser()
        //{
        //    // Arrange
        //    var user = new User { Id = 1, UserName = "User1" };
        //    var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
        //    user.Roles = new List<Role> { role };
        //    _context.User.Add(user);
        //    _context.Role.Add(role);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Edit(1);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.AreEqual(user, viewResult.Model);
        //    ClassicAssert.IsNotNull(viewResult.ViewData["Roles"]);
        //}

        [Test]
        public async Task Edit_Get_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        //[Test]
        //public async Task Edit_Post_ValidModel_RedirectsToIndex()
        //{
        //    // Arrange
        //    var user = new User { Id = 1, UserName = "User1" };
        //    var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
        //    user.Roles = new List<Role> { role };
        //    _context.User.Add(user);
        //    _context.Role.Add(role);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Edit(1, user, new List<int> { 1 });

        //    // Assert
        //    ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
        //    var redirectResult = result as RedirectToActionResult;
        //    ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        //}

        //[Test]
        //public async Task Edit_Post_InvalidModel_ReturnsView()
        //{
        //    // Arrange
        //    var user = new User { Id = 1 };
        //    _controller.ModelState.AddModelError("UserName", "Required");
        //    var role = new Role { Id = 1, UserRole = Enums.UserRole.Admin };
        //    _context.Role.Add(role);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Edit(1, user, new List<int> { 1 });

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.AreEqual(user, viewResult.Model);
        //    ClassicAssert.IsNotNull(viewResult.ViewData["Roles"]);
        //}

        //[Test]
        //public async Task Delete_Get_ValidId_ReturnsViewWithUser()
        //{
        //    // Arrange
        //    var user = new User { Id = 1, UserName = "User1" };
        //    _context.User.Add(user);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.Delete(1);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<ViewResult>(result);
        //    var viewResult = result as ViewResult;
        //    ClassicAssert.AreEqual(user, viewResult.Model);
        //}

        [Test]
        public async Task Delete_Get_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Delete(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        //[Test]
        //public async Task DeleteConfirmed_ValidId_RedirectsToIndex()
        //{
        //    // Arrange
        //    var user = new User { Id = 1, UserName = "User1" };
        //    _context.User.Add(user);
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _controller.DeleteConfirmed(1);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
        //    var redirectResult = result as RedirectToActionResult;
        //    ClassicAssert.AreEqual("Index", redirectResult.ActionName);
        //}
    }
} 