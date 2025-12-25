using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Data;
using System.Diagnostics;

namespace RealtorsPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
         

            

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public async Task<IActionResult> Agent()
        {
            var agents = await _context.Users
                .Where(u => u.UserType == "Agent")
                .Take(12)
                .ToListAsync();

            ViewBag.Agents = agents;
            return View();
        }

        public async Task<IActionResult> Properties(string? search, string? category, string? type, string? city, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Properties
                .Where(p => p.IsApproved)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.User)
                .AsQueryable();

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

        public IActionResult Blog()
        {
            return View();
        }

         
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Contact");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}