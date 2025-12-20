using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Data;
using System.Security.Claims;

namespace RealtorsPortal.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userProperties = await _context.Properties
                .Where(p => p.UserId == userId)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToListAsync();

            var userPackage = await _context.UserPackages
                .Include(up => up.Package)
                .Where(up => up.UserId == userId && up.IsActive)
                .FirstOrDefaultAsync();

            ViewBag.Properties = userProperties;
            ViewBag.UserPackage = userPackage;
            ViewBag.PropertyCount = userProperties.Count;

            return View();
        }

        public async Task<IActionResult> MyProperties()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var properties = await _context.Properties
                .Where(p => p.UserId == userId)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(properties);
        }

        public async Task<IActionResult> EditProperty(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var property = await _context.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (property == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Property deleted successfully!";
            }

            return RedirectToAction("MyProperties");
        }

        public async Task<IActionResult> Messages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Property inquiries for user's properties
            var inquiries = await _context.ContactMessages
                .Include(c => c.Property)
                .Where(c => c.Property.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(inquiries);
        }

        public IActionResult Packages()
        {
            var packages = _context.Packages.Where(p => p.IsActive).ToList();
            return View(packages);
        }
    }
}