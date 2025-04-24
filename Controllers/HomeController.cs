using System.Diagnostics;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var viewModel = new ErrorViewModel
            {
                StatusCode = statusCode
            };

            switch (statusCode)
            {
                case 403:
                    viewModel.Message = "You are not allowed.";
                    break;
                case 404:
                    viewModel.Message = "Sorry, the page you are looking for could not be found.";
                    break;
                case 500:
                    viewModel.Message = "Oops! Something went wrong on our end.";
                    break;
                default:
                    viewModel.Message = "An unexpected error occurred.";
                    break;
            }

            return View(viewModel);
        }

    }
}
