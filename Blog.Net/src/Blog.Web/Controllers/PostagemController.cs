using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Business.Services;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class PostagemController : BaseController
    {

        private readonly IPostagemRepository _postagemRepository;
        private readonly IPostagemService _postagemService;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;

        public PostagemController(IPostagemRepository repository,
                                  IPostagemService service,
                                  IAutorRepository autorRepository,
                                  IAutorService autorService,
                                  INotificador notificador,
                                  IAppIdentityUser user) : base(notificador, user)
        {
            _postagemRepository = repository;
            _postagemService = service;

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

            var posts = await _postagemRepository.ObterTodasPostagem();
            return View(posts);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dados-da-postagem/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            var postagem = await _postagemRepository.ObterPostagem(id);

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

                await _postagemService.Adicionar(postagem);

                return RedirectToAction("Index");

            }
            return View(postagem);
        }


        [Authorize]
        [HttpGet, ActionName("Edit")]
        [Route("editar-postagem/{id:guid}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Postagem postagem = await _postagemRepository.ObterPostagem(Guid.Parse(id.ToString()));

            if (UserAdmin == false && UserId != postagem.IdAutor)
            {
                return Unauthorized();
            }


            if (postagem == null)
            {
                return NotFound();
            }
            return View(postagem);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-postagem/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Titulo,Conteudo,DataCriacao,DataAtualizacao,IdAutor,Id")] Postagem postagem)
        {
            if (id != postagem.Id)
            {
                return NotFound();
            }

            if (UserAdmin == false && UserId != postagem.IdAutor)
            {
                return Unauthorized();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    postagem.DataAtualizacao = DateTime.Now;
                    await _postagemRepository.Atualizar(postagem);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostagemExists(postagem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(await _autorRepository.ObterTodos(), "Id", "Email", postagem.IdAutor);
            return View(postagem);
        }


        [Authorize]
        [HttpGet, ActionName("Delete")]
        [Route("excluir-postagem/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var postagem = await _postagemRepository.ObterPorId(id);
            if (postagem == null)
            {
                return NotFound();
            }

            if (UserAdmin == false && UserId != postagem.IdAutor)
            {
                return Unauthorized();
            }


            return View(postagem); 
        }


        [HttpPost, ActionName("DeleteConfirmado")]
        [ValidateAntiForgeryToken]
        [Route("excluir-postagem/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmado(Guid id)
        {
            var postagem = await _postagemRepository.ObterPorId(id);
            if (postagem == null)
            {
                return NotFound();
            }

            if (UserAdmin == false && UserId != postagem.IdAutor)
            {
                return Unauthorized();
            }


            await _postagemService.Remover(id);

            return RedirectToAction("Index");
        }



        private bool PostagemExists(Guid id)
        {
            return _postagemRepository.ObterPostagem(id) != null ? true : false;
        }


    }
}
