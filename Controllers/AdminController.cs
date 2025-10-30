using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 🏠 Admin Dashboard
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            ViewBag.TotalBooks = await _context.Books.CountAsync();
            ViewBag.AvailableBooks = await _context.Books.CountAsync(b => b.IsAvailable);
            ViewBag.IssuedBooks = await _context.Books.CountAsync(b => !b.IsAvailable);
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalBorrowed = await _context.Borrows.CountAsync();

            return View();
        }

        // 📚 Manage Books
        public async Task<IActionResult> ManageBooks()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // ➕ Add Book (GET)
        [HttpGet]
        public IActionResult AddBook()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // ➕ Add Book (POST) – Admin manually enters Book ID
        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            // ✅ Check if the entered Book ID already exists
            var existingBook = await _context.Books.FindAsync(book.Id);
            if (existingBook != null)
            {
                ModelState.AddModelError("Id", "Book ID already exists. Please use a different ID.");
                return View(book);
            }

            if (ModelState.IsValid)
            {
                book.IsAvailable = true;
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageBooks));
            }

            return View(book);
        }

        // ✏️ Edit Book (GET)
        [HttpGet]
        public async Task<IActionResult> EditBook(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // ✏️ Edit Book (POST)
        [HttpPost]
        public async Task<IActionResult> EditBook(Book book)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageBooks));
            }

            return View(book);
        }

        // 🗑️ Delete Book
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageBooks));
        }
    }
}
