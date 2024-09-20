using Blog.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Include(p => p.Autor).ToListAsync();
            return View(posts);
        }
    }
}
