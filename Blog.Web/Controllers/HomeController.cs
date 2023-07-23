using Blog.Web.Data;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public HomeController(ILogger<HomeController> logger, BlogDbContext blogDbContext)
        {
            _logger = logger;
            _blogDbContext = blogDbContext;
        }

        public IActionResult Index()
        {
            // Get the user ID of the currently logged-in user
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Query the database to retrieve the user data based on the user ID
            var user = _blogDbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);

            if (user == null)
            {
                // User not found, handle the scenario (e.g., redirect to login page)
                return RedirectToAction("Loginn", "User");
            }
            

            // Pass the user data to the view
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
