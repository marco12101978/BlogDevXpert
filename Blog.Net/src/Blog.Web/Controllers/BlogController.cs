using Blog.Data.Context;
using Blog.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly MeuDbContext _context;

        public BlogController(MeuDbContext context)
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
