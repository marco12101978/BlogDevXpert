using Blog.Business.Intefaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class PostagemController : BaseController
    {

        private readonly IPostagemRepository _repository;
        private readonly IPostagemService _service;

        public PostagemController(IPostagemRepository repository,
                                  IPostagemService service,
                                  INotificador notificador,
                                  IAppIdentityUser user) : base(notificador, user)
        {
            _repository = repository;
            _service = service;
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Postagem post)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(post); 
        }


        public async Task<IActionResult> Index()
        {
            var posts = await _repository.ObterTodasPostagem();
            return View(posts);

        }

        public async Task<IActionResult> Detalhes(Guid id)
        {
            var postagem = await _repository.ObterPostagem(id);

            if (postagem == null)
            {
                return NotFound(); 
            }

            return View(postagem);
        }

    }
}
