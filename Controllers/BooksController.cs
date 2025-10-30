using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // 📚 Show all books (Anyone can see)
        public async Task<IActionResult> Index(string? search)
        {
            var books = from b in _context.Books select b;

            if (!string.IsNullOrEmpty(search))
            {
                string lowerSearch = search.ToLower();
                books = books.Where(b =>
                    b.Title.ToLower().Contains(lowerSearch) ||
                    b.Author.ToLower().Contains(lowerSearch) ||
                    b.Category.ToLower().Contains(lowerSearch)
                );
            }

            return View(await books.ToListAsync());
        }

        // 📘 Issue Book
        [HttpPost]
        public async Task<IActionResult> Issue(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login", "Auth");

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null || !book.IsAvailable)
                return RedirectToAction(nameof(Index));

            var alreadyBorrowed = await _context.Borrows
                .FirstOrDefaultAsync(b => b.BookId == id && b.ReturnDate == null && b.UserEmail == userEmail);

            if (alreadyBorrowed != null)
            {
                TempData["Error"] = "You have already issued this book.";
                return RedirectToAction(nameof(Index));
            }

            var borrow = new Borrow
            {
                BookId = id,
                UserEmail = userEmail,
                IssueDate = DateTime.Now
            };

            book.IsAvailable = false;
            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Book issued successfully!";
            return RedirectToAction(nameof(MyBooks));
        }

        // 📗 My Books
        public async Task<IActionResult> MyBooks()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login", "Auth");

            var myBooks = await _context.Borrows
                .Include(b => b.Book)
                .Where(b => b.UserEmail == userEmail && b.ReturnDate == null)
                .ToListAsync();

            return View(myBooks);
        }

        // 🔙 Return a Book
        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (borrow != null && borrow.ReturnDate == null)
            {
                borrow.ReturnDate = DateTime.Now;
                borrow.Book.IsAvailable = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyBooks));
        }

        // ⚙️ Manage Books (Admin)
        public async Task<IActionResult> Manage()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View(await _context.Books.ToListAsync());
        }

        // ➕ Add new book (Admin only)
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                book.IsAvailable = true;
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(book);
        }

        // ✏️ Edit Book (Admin)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(book);
        }

        // 🗑️ Delete Book (Admin)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Manage));
        }

        // 🔍 Book Details
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
