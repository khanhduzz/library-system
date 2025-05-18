using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    public class BaseController : Controller
    {
        protected int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
