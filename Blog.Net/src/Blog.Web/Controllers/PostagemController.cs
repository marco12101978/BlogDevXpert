using AutoMapper;
using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Blog.Web.Controllers
{
    public class PostagemController : BaseController
    {

        private readonly IPostagemRepository _repository;
        private readonly IPostagemService _service;
        private readonly IMapper _mapper;

        public PostagemController(IPostagemRepository repository,
                                  IPostagemService service,
                                  INotificador notificador,
                                  IMapper mapper,
                                  IAppIdentityUser user) : base(notificador, user)
        {
            _repository = repository;
            _service = service;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Authors = _context.Authors.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Postagem post)
        {
            if (ModelState.IsValid)
            {
                //post.DataCriacao = DateTime.Now; 
                //_context.Add(post);
                //await _context.SaveChangesAsync(); 
                return RedirectToAction("Index");
            }
            return View(post); 
        }


        public async Task<IActionResult> Index()
        {
            //var posts = await _context.Posts.Include(p => p.Autor).ToListAsync();
            // return View(posts);

            var posts = await _repository.ObterPostagem(UserId);
            return View(posts);

        }
    }
}
