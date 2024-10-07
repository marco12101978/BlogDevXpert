using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Business.Services;
using Blog.Data.Context;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class AutorController : BaseController
    {

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;


        public AutorController(IAutorRepository autorRepository,
                                     IAutorService autorService,
                                     INotificador notificador,
                                     IAppIdentityUser user) : base(notificador, user)
        {
            _autorRepository = autorRepository;
            _autorService = autorService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("lista-de-autores")]
        public async Task<IActionResult> Index()
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;
            return View(await _autorRepository.ObterTodos());
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("dados-do-autor/{id:guid}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            var autor =  _autorRepository.ObterPorId(id ?? Guid.Empty).Result;

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [Authorize]
        [HttpGet]
        [Route("novo-autor/{id:guid}")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-autor/{id:guid}")]
        public async Task<IActionResult> Create([Bind("Nome,Email,Biografia,Id")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                autor.Id = UserId;
                await _autorService.Adicionar(autor);
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        [Authorize]
        [HttpGet]
        [Route("editar-autor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _autorRepository.ObterPorId(id ?? Guid.Empty);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-autor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Nome,Email,Biografia,Id")] Autor autor)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _autorDB = _autorRepository.ObterPorId(id).Result;

                    await _autorRepository.Atualizar(_autorDB);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id))
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
            return View(autor);
        }

        [Authorize]
        [HttpGet]
        [Route("deletar-autor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Autor autor = _autorRepository.ObterPorId(id ?? Guid.Empty).Result;

            if (UserAdmin == false && UserId != autor.Id )
            {
                return Unauthorized();
            }

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("deletar-autor/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var autor = await _autorRepository.ObterPorId(id);
            if (autor != null)
            {
                await _autorRepository.Remover(autor.Id);
            }

            return RedirectToAction("Index", "Autor");
        }

        private bool AutorExists(Guid id)
        {
            return  _autorRepository.ObterPorId(id) != null ? true : false;
        }
    }
}
