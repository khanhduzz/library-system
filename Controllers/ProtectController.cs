using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    public class ProtectController
    {
        [Authorize]
        public class HomeController : Controller
        {
            public IActionResult Index() => View();
        }

        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            public IActionResult Index() => View();
        }
    }
}
