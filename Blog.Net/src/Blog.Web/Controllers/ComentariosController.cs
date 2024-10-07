using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class ComentariosController : BaseController
    {
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IComentarioService _comentarioService;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;

        public ComentariosController(IComentarioRepository repository,
                                     IComentarioService service,
                                     IAutorRepository autorRepository,
                                     IAutorService autorService,
                                     INotificador notificador,
                                     IAppIdentityUser user) : base(notificador, user)
        {
            _comentarioRepository = repository;
            _comentarioService = service;

            _autorRepository = autorRepository;
            _autorService = autorService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("lista-de-comentarios")]
        public async Task<IActionResult> Index()
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            var posts = await _comentarioRepository.ObterTodos();
            return View(posts);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dados-do-comentario/{id:guid}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;


            Comentario comentario = _comentarioRepository.ObterComentario(id ?? Guid.Empty).Result;

            if (comentario == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Excluir Comentário";
            return View(comentario);
        }

        [Authorize]
        [HttpGet]
        [Route("novo-comentario/{id:guid}")]
        public async Task<IActionResult> Create(Guid id)
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


            ViewBag.idPostagem = id.ToString();

            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-comentario/{id:guid}")]
        public async Task<IActionResult> Create([Bind("Conteudo,IdPostagem")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {

                comentario.Id = Guid.NewGuid();
                comentario.Conteudo = comentario.Conteudo;
                comentario.IdPostagem = comentario.IdPostagem;
                comentario.DataPostagem = DateTime.Now;
                comentario.IdAutor = UserId;
                comentario.NomeAutor = UserName;

                await _comentarioService.Adicionar(comentario);

                if (!OperacaoValida())
                {
                    ViewBag.idPostagem = comentario.Id.ToString();
                    return View(comentario);
                }

                return RedirectToAction("Details", "Postagem", new { id = comentario.IdPostagem });
            }
            
            return View(comentario);
        }

        [Authorize]
        [HttpGet]
        [Route("editar-comentario/{id:guid}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _comentarioRepository.ObterPorId(id ?? Guid.Empty);
            if (comentario == null)
            {
                return NotFound();
            }

            if (UserAdmin == false && UserId != comentario.IdAutor)
            {
                return Unauthorized();
            }


            return View(comentario);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-comentario/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Conteudo,DataPostagem,NomeAutor,IdAutor,IdPostagem,Id")] Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _comentarioDB = _comentarioRepository.ObterPorId(id).Result;

                    if (UserAdmin == false && UserId != _comentarioDB.IdAutor)
                    {
                        return Unauthorized();
                    }


                    if (_comentarioDB != null)
                    {
                        _comentarioDB.Conteudo = comentario.Conteudo;
                    }

                    await _comentarioService.Atualizar(_comentarioDB);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentarioExists(comentario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Postagem", new { id = comentario.IdPostagem });
            }
            ViewData["IdPostagem"] = id.ToString() ;
            return View(comentario);
        }

        [Authorize]
        [HttpGet]
        [Route("deletar-comentario/{id:guid}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comentario comentario = _comentarioRepository.ObterComentario(id ?? Guid.Empty).Result;

            if (comentario == null)
            {
                return NotFound();
            }

            if (UserAdmin == false && UserId != comentario.IdAutor)
            {
                return Unauthorized();
            }


            return View(comentario);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("deletar-comentario/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var comentario = await _comentarioRepository.ObterPorId(id);
            if (comentario != null)
            {
                if (UserAdmin == false && UserId != comentario.IdAutor)
                {
                    return Unauthorized();
                }

                await _comentarioService.Remover(comentario.Id);
                return RedirectToAction("Details", "Postagem", new { id = comentario.IdPostagem });
            }

            return RedirectToAction("Index", "Postagem");
        }

        private bool ComentarioExists(Guid id)
        {
            return _comentarioRepository.ObterPorId(id) != null ? true : false;
        }
    }
}
