using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class PostagemController : BaseController
    {

        private readonly IPostagemRepository _repository;
        private readonly IPostagemService _service;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;

        public PostagemController(IPostagemRepository repository,
                                  IPostagemService service,
                                  IAutorRepository autorRepository,
                                  IAutorService autorService,
                                  INotificador notificador,
                                  IAppIdentityUser user) : base(notificador, user)
        {
            _repository = repository;
            _service = service;

            _autorRepository = autorRepository;
            _autorService = autorService;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("lista-de-postagem")]
        public async Task<IActionResult> Index()
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            var posts = await _repository.ObterTodasPostagem();
            return View(posts);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dados-da-postagem/{id:guid}")]
        public async Task<IActionResult> Detalhes(Guid id)
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            var postagem = await _repository.ObterPostagem(id);

            if (postagem == null)
            {
                return NotFound();
            }

            return View(postagem);
        }


        [Authorize]
        [HttpGet]
        [Route("nova-postagem")]
        public async Task<IActionResult> Create()
        {
            var _autor = await _autorRepository.Buscar(p => p.Id == UserId);

            if (_autor == null || !_autor.Any())
            {
                var _insAutor = new Autor
                {
                    Email = UserName,
                    Nome = UserName,
                    Id = UserId,
                    Biografia = ""
                };


                await _autorService.Adicionar(_insAutor);

                //return NotFound($"Nenhum autor encontrado. {UserId}-{UserName}"); 
            }

            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("nova-postagem")]
        public async Task<IActionResult> Create([Bind("Titulo,Conteudo,IdAutor")] Postagem postagem)
        {
            if (ModelState.IsValid)
            {
                
                postagem.Id = Guid.NewGuid();
                postagem.DataCriacao = DateTime.Now;
                postagem.IdAutor = UserId;

                await _repository.Adicionar(postagem);

                return RedirectToAction("Index");

            }
            return View(postagem);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var postagem = await _repository.ObterPorId(id);
            if (postagem == null)
            {
                return NotFound();
            }

            return View(postagem); 
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteConfirmado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmado(Guid id)
        {
            var postagem = await _repository.ObterPorId(id);
            if (postagem == null)
            {
                return NotFound();
            }

            await _repository.Remover(id);

            return RedirectToAction("Index");
        }





    }
}
