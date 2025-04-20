using LibrarySystem.Controllers;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace LibrarySystem.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<ILogger<HomeController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object);
        }

        [Test]
        public void Index_ReturnsView()
        {
            // Act
            var result = _controller.Index();

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Privacy_ReturnsView()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Error_ReturnsViewWithErrorModel()
        {
            // Arrange
            int statusCode = 404;

            // Act
            var result = _controller.Error(statusCode);

            // Assert
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.IsInstanceOf<ErrorViewModel>(viewResult.Model);
            var model = viewResult.Model as ErrorViewModel;
            ClassicAssert.AreEqual(statusCode, model.StatusCode);
            ClassicAssert.IsNotEmpty(model.Message);
        }

        [Test]
        public void Error_403_ReturnsCorrectMessage()
        {
            // Arrange
            int statusCode = 403;

            // Act
            var result = _controller.Error(statusCode);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as ErrorViewModel;
            ClassicAssert.AreEqual("You are not allowed.", model.Message);
        }

        [Test]
        public void Error_404_ReturnsCorrectMessage()
        {
            // Arrange
            int statusCode = 404;

            // Act
            var result = _controller.Error(statusCode);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as ErrorViewModel;
            ClassicAssert.AreEqual("Sorry, the page you are looking for could not be found.", model.Message);
        }

        [Test]
        public void Error_500_ReturnsCorrectMessage()
        {
            // Arrange
            int statusCode = 500;

            // Act
            var result = _controller.Error(statusCode);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as ErrorViewModel;
            ClassicAssert.AreEqual("Oops! Something went wrong on our end.", model.Message);
        }

        [Test]
        public void Error_UnknownStatusCode_ReturnsDefaultMessage()
        {
            // Arrange
            int statusCode = 999;

            // Act
            var result = _controller.Error(statusCode);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as ErrorViewModel;
            ClassicAssert.AreEqual("An unexpected error occurred.", model.Message);
        }
    }
} 