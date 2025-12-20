using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Data;
using RealtorsPortal.Models;

namespace RealtorsPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Dashboard statistics
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalProperties = await _context.Properties.CountAsync(),
                PendingProperties = await _context.Properties.CountAsync(p => !p.IsApproved),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalAgents = await _context.Users.CountAsync(u => u.UserType == "Agent"),
                TotalSellers = await _context.Users.CountAsync(u => u.UserType == "PrivateSeller"),
                TodayPayments = await _context.Payments
                    .Where(p => p.PaymentDate.Date == DateTime.Today && p.Status == "Completed")
                    .SumAsync(p => p.Amount)
            };

            // Recent properties
            var recentProperties = await _context.Properties
                .Include(p => p.User)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToListAsync();

            ViewBag.Stats = stats;
            ViewBag.RecentProperties = recentProperties;

            return View();
        }

        // Users Management
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        // Properties Management
        public async Task<IActionResult> Properties(string? status)
        {
            var query = _context.Properties
                .Include(p => p.User)
                .Include(p => p.Category)
                .AsQueryable();

            if (status == "pending")
                query = query.Where(p => !p.IsApproved);
            else if (status == "approved")
                query = query.Where(p => p.IsApproved);

            var properties = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                property.IsApproved = true;
                _context.Update(property);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Property approved successfully!";
            }

            return RedirectToAction("Properties");
        }

        [HttpPost]
        public async Task<IActionResult> RejectProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Property rejected and removed!";
            }

            return RedirectToAction("Properties");
        }

        // Categories Management
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedAt = DateTime.Now;
                _context.Add(category);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Category added successfully!";
            }

            return RedirectToAction("Categories");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Category deleted successfully!";
            }

            return RedirectToAction("Categories");
        }

        // Packages Management
        public async Task<IActionResult> Packages()
        {
            var packages = await _context.Packages.ToListAsync();
            return View(packages);
        }

        // Reports
        public async Task<IActionResult> Reports()
        {
            var payments = await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Package)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return View(payments);
        }

        // Settings
        public IActionResult Settings()
        {
            return View();
        }
    }
}