using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class PostagemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostagemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Postagem post)
        {
            if (ModelState.IsValid)
            {
                post.DataCriacao = DateTime.Now; 
                _context.Add(post);
                await _context.SaveChangesAsync(); 
                return RedirectToAction("Index");
            }
            return View(post); 
        }


        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Include(p => p.Autor).ToListAsync();
            return View(posts);
        }
    }
}
