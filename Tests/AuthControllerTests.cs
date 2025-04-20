using LibrarySystem.Controllers;
using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibrarySystem.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _controller;
        private LibrarySystemContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibrarySystemContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new LibrarySystemContext(options);
            // Clear the database before each test
            _context.Database.EnsureDeleted();
            _controller = new AuthController(_context);
        }

        [Test]
        public void Login_Get_ReturnsView()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Login_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new LoginViewModel { Username = "", Password = "" };
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            ClassicAssert.AreEqual(model, ((ViewResult)result).Model);
        }

        [Test]
        public async Task Login_Post_InvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            var model = new LoginViewModel { Username = "test", Password = "wrong" };
            var user = new User
            {
                UserName = "test",
                Password = "correct", // correct password in DB, wrong in input
                Email = "email@mail.com",
                Name = "name"
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            ClassicAssert.IsFalse(_controller.ModelState.IsValid);
        }

        //[Test]
        //public async Task Login_Post_ValidCredentials_RedirectsToHome()
        //{
        //    // Arrange
        //    var model = new LoginViewModel { Username = "test", Password = "correct" };
        //    var user = new User { UserName = "test", Password = "correct", Email = "email@mail.com", Name = "name", Roles = new List<Role>() };
        //    _context.User.Add(user);
        //    await _context.SaveChangesAsync();

        //    var mockAuthService = new Mock<IAuthenticationService>();
        //    mockAuthService.Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);
        //    _controller.ControllerContext.HttpContext = new DefaultHttpContext { RequestServices = new ServiceProviderMock(mockAuthService.Object) };

        //    // Act
        //    var result = await _controller.Login(model);

        //    // Assert
        //    ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
        //    ClassicAssert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        //    ClassicAssert.AreEqual("Home", ((RedirectToActionResult)result).ControllerName);
        //}

        //[Test]
        //public async Task Logout_SignsOutAndRedirectsToLogin()
        //{
        //    // Arrange
        //    var mockAuthService = new Mock<IAuthenticationService>();
        //    mockAuthService.Setup(a => a.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);
        //    _controller.ControllerContext.HttpContext = new DefaultHttpContext { RequestServices = new ServiceProviderMock(mockAuthService.Object) };

        //    // Act
        //    var result = await _controller.Logout();

        //    // Assert
        //    ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
        //    ClassicAssert.AreEqual("Login", ((RedirectToActionResult)result).ActionName);
        //    ClassicAssert.AreEqual("Auth", ((RedirectToActionResult)result).ControllerName);
        //}
    }

    public class ServiceProviderMock : IServiceProvider
    {
        private readonly IAuthenticationService _authService;

        public ServiceProviderMock(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IAuthenticationService))
                return _authService;
            return null;
        }
    }
}