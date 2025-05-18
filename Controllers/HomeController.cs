using System.Diagnostics;
using LibrarySystem.Models;
using LibrarySystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Generic;

namespace LibrarySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibrarySystemContext _context;

        public HomeController(ILogger<HomeController> logger, LibrarySystemContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Book.Include(b => b.Author).ToListAsync();
            var authors = await _context.Author.ToListAsync();

            var viewModel = new HomeViewModel
            {
                Books = books,
                Authors = authors
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode, string? message)
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

            if (!string.IsNullOrEmpty(message))
            {
                viewModel.Message = message;
            }

            return View(viewModel);
        }
    }
}
