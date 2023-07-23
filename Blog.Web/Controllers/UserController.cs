using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly BlogDbContext _blogDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(BlogDbContext blogDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _blogDbContext = blogDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Loginn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(loginUser loginuser)
        {
            var user = _blogDbContext.Users.FirstOrDefault(u =>
                u.Name == loginuser.Name && u.Password == loginuser.Password);

            if (user == null)
            {
                // Authentication not successful, redirect to the Login page
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View("Loginn");
            }

            var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Assuming user.Id is of type Guid
                new Claim(ClaimTypes.Name, user.Name),
                // Add other claims as needed
            };

            // Create the authentication ticket and sign in the user
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or login page after logout
            return RedirectToAction("Loginn");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewRegister(loginUser loginUser)
        {

            string uniqueFileName = null;
            Console.WriteLine(loginUser.Imgfile);
            if (loginUser.Imgfile != null && loginUser.Imgfile.Length > 0)
            {
                // Save the uploaded image to a specific location (you can change this as needed)
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UserImage");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + loginUser.Imgfile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await loginUser.Imgfile.CopyToAsync(stream);
                }
            

            var user = new User
            {
                Name = loginUser.Name,
                Email = loginUser.Email,
                Password = loginUser.Password,
                Img = uniqueFileName // Save the image path to the Img property in the User model
            };

            _blogDbContext.Users.Add(user);
            await _blogDbContext.SaveChangesAsync();

            // Redirect to the login view after successful registration
            return RedirectToAction("Loginn");
        }

            // If the model state is not valid, return to the registration view to display errors
            return View("Register", loginUser);
    }
    }
}
