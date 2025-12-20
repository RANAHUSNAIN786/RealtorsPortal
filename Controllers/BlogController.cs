using Microsoft.AspNetCore.Mvc;
using RealtorsPortal.Data;

namespace RealtorsPortal.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Blog
        public IActionResult Index()
        {
            // Static blog data for now
            var blogPosts = new List<BlogPost>
            {
                new BlogPost {
                    Id = 1,
                    Title = "10 Tips for First Time Home Buyers",
                    Excerpt = "Essential advice for those buying their first home in Pakistan...",
                    ImageUrl = "/user/images/image_1.jpg",
                    Date = DateTime.Now.AddDays(-5),
                    Author = "Admin"
                },
                new BlogPost {
                    Id = 2,
                    Title = "How to Get the Best Mortgage Rates",
                    Excerpt = "Learn how to secure the best mortgage rates in the current market...",
                    ImageUrl = "/user/images/image_2.jpg",
                    Date = DateTime.Now.AddDays(-10),
                    Author = "Agent Ali"
                },
                new BlogPost {
                    Id = 3,
                    Title = "Commercial Real Estate Trends 2024",
                    Excerpt = "Latest trends in commercial real estate across major Pakistani cities...",
                    ImageUrl = "/user/images/image_3.jpg",
                    Date = DateTime.Now.AddDays(-15),
                    Author = "Market Expert"
                }
            };

            return View(blogPosts);
        }

        // GET: /Blog/Details/5
        public IActionResult Details(int id)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Title = "Sample Blog Post Title",
                Content = "<p>This is the full content of the blog post. You can write detailed articles here.</p><p>Include images, videos, and other media content.</p>",
                ImageUrl = "/user/images/image_1.jpg",
                Date = DateTime.Now.AddDays(-5),
                Author = "Admin",
                Category = "Real Estate",
                Tags = new List<string> { "Property", "Investment", "Tips" }
            };

            ViewBag.RelatedPosts = new List<BlogPost>
            {
                new BlogPost { Id = 2, Title = "Related Post 1", ImageUrl = "/user/images/image_2.jpg" },
                new BlogPost { Id = 3, Title = "Related Post 2", ImageUrl = "/user/images/image_3.jpg" }
            };

            return View(blogPost);
        }
    }

    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }
}