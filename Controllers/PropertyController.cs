using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Data;

namespace RealtorsPortal.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Property
        public async Task<IActionResult> Index(string? search, string? category, string? type, string? city, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Properties
                .Where(p => p.IsApproved)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.User)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Title.Contains(search) || p.Description.Contains(search));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Name == category);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.PropertyType == type);

            if (!string.IsNullOrEmpty(city))
                query = query.Where(p => p.City == city);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var properties = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();

            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            ViewBag.Cities = await _context.Properties.Select(p => p.City).Distinct().ToListAsync();
            ViewBag.PropertyTypes = await _context.Properties.Select(p => p.PropertyType).Distinct().ToListAsync();

            return View(properties);
        }

        // GET: /Property/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            property.ViewCount++;
            _context.Update(property);
            await _context.SaveChangesAsync();

            return View(property);
        }
    }
}