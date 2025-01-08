using AutoMapper;
using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AutorController : BaseController
    {
        private readonly IMapper _mapper;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;


        public AutorController(IMapper mapper,
                               IAutorRepository autorRepository,
                               IAutorService autorService,
                               INotificador notificador,
                               IAppIdentityUser user) : base(notificador, user)
        {
            _mapper = mapper;
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

            return View(_mapper.Map<IEnumerable<AutorViewModel>>(await _autorRepository.ObterTodos()));
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

            AutorViewModel? autor = await ObterAutor(id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [Authorize]
        [HttpGet]
        [Route("novo-autor")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-autor")]
        public async Task<IActionResult> Create([Bind("Nome,Email,Biografia,Id")] AutorViewModel autor)
        {
            if (ModelState.IsValid)
            {
                autor.Id = UserId;
                await _autorService.Adicionar(_mapper.Map<Autor>(autor));
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

            AutorViewModel? autor = await ObterAutor(id);

            if (autor == null)
            {
                return NotFound();
            }

            if (UserAdmin == false && UserId != autor.Id)
            {
                return Unauthorized();
            }

            return View(autor);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-autor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Nome,Email,Biografia,Id")] AutorViewModel autor)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                AutorViewModel? _autorDB = await ObterAutor(id);

                if (_autorDB == null)
                {
                    return NotFound();
                }

                _autorDB.Email = autor.Email;
                _autorDB.Nome = autor.Nome;
                _autorDB.Biografia = autor.Biografia;

                if (UserAdmin == false && UserId != autor.Id)
                {
                    return Unauthorized();
                }

                await _autorRepository.Atualizar(_mapper.Map<Autor>(_autorDB));

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

            AutorViewModel? autor = await ObterAutor(id);

            if (UserAdmin == false && UserId != autor?.Id )
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
            AutorViewModel? autor = await ObterAutor(id);
            if (autor != null)
            {
                if (UserAdmin == false && UserId != autor.Id)
                {
                    return Unauthorized();
                }

                await _autorRepository.Remover(autor.Id);
            }

            return RedirectToAction("Index", "Autor");
        }

        private bool AutorExists(Guid id)
        {
            return  _autorRepository.ObterPorId(id) != null ? true : false;
        }

        private async Task<AutorViewModel?> ObterAutor(Guid? id)
        {
            if (id == null)
                return null;

            var _autor = _mapper.Map<AutorViewModel>(await _autorRepository.ObterPorId(id ?? Guid.Empty));
            return _autor;
        }

    }
}
