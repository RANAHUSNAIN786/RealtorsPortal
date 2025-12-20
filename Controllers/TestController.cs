using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealtorsPortal.Data;

namespace RealtorsPortal.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CheckDb()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                var users = await _context.Users.ToListAsync();

                ViewBag.Message = "Database connected successfully!";
                ViewBag.CategoryCount = categories.Count;
                ViewBag.UserCount = users.Count;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Database connection failed!";
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}