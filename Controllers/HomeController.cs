using Microsoft.AspNetCore.Mvc;

namespace KaraokeAPI.Controllers
{
    public class HomeController : Controller
    {
        // GET /
        [HttpGet("/")]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "home.html"), "text/html");
        }

        // GET /home
        [HttpGet("/home")]
        public IActionResult Home()
        {
            return Redirect("/");
        }
    }
}
