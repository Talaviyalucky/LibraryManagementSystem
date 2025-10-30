using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // 🧩 Login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🧩 Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // ✅ Store session data
            HttpContext.Session.SetString("UserEmail", user.Email ?? user.Username); // fallback if Email is null
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            // ✅ Redirect based on role
            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin"); // 👈 updated line
            else
                return RedirectToAction("Index", "Home"); // 👈 regular user goes to home/books
        }

        // 🚪 Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // 🧩 Register (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 🧩 Register (POST)
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
    }
}
