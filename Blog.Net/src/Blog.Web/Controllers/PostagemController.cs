﻿using AutoMapper;
using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize]
    public class PostagemController : BaseController
    {
        private readonly IMapper _mapper;

        private readonly IPostagemRepository _postagemRepository;
        private readonly IPostagemService _postagemService;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;


        public PostagemController(IMapper mapper,
                                  IPostagemRepository repository,
                                  IPostagemService service,
                                  IAutorRepository autorRepository,
                                  IAutorService autorService,
                                  INotificador notificador,
                                  IAppIdentityUser user) : base(notificador, user)
        {
            _mapper = mapper;

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

            return View(_mapper.Map<List<PostagemViewModel>>(await _postagemRepository.ObterTodasPostagem()));

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dados-da-postagem/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            PostagemViewModel postagemViewModel = await ObterPostagem(id);

            if (postagemViewModel == null)
            {
                return NotFound();
            }

            return View(postagemViewModel);
        }


        [HttpGet]
        [Route("nova-postagem")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Autor> _autor = await _autorRepository.Buscar(p => p.Id == UserId);

            await CriarAutorSeNaoExistir();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("nova-postagem")]
        public async Task<IActionResult> Create([Bind("Titulo,Conteudo")] PostagemViewModel postagemViewModel)
        {
            if (ModelState.IsValid)
            {
                await CriarAutorSeNaoExistir();

                postagemViewModel.Id = Guid.NewGuid();
                postagemViewModel.DataCriacao = DateTime.Now;
                postagemViewModel.IdAutor = UserId;

                await _postagemService.Adicionar(_mapper.Map<Postagem>(postagemViewModel));

                return RedirectToAction("Index");

            }
            return View(postagemViewModel);
        }


        [HttpGet, ActionName("Edit")]
        [Route("editar-postagem/{id:guid}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)  return NotFound();

            PostagemViewModel? postagem = await ObterPostagem(id);

            if(postagem == null) return NotFound();

            if (!UsuarioPodeModificar(postagem)) return Unauthorized();

            if (postagem == null) return NotFound();
            
            return View(postagem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-postagem/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Titulo,Conteudo,DataCriacao,IdAutor,Id")] PostagemViewModel postagem)
        {
            if (id != postagem.Id)  return NotFound();

            if (!UsuarioPodeModificar(postagem)) return Unauthorized();

            if (ModelState.IsValid)
            {
                postagem.DataAtualizacao = DateTime.Now;
                await _postagemRepository.Atualizar(_mapper.Map<Postagem>(postagem));

                return RedirectToAction(nameof(Index));
            }

            return View(postagem);
        }


        [HttpGet, ActionName("Delete")]
        [Route("excluir-postagem/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            PostagemViewModel? postagem = await ObterPostagem(id);

            if (postagem == null) return NotFound();

            if (!UsuarioPodeModificar(postagem)) return Unauthorized();

            return View(postagem); 
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-postagem/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmado(Guid id)
        {
            PostagemViewModel? postagem = await ObterPostagem(id);

            if (postagem == null) return NotFound();

            if (!UsuarioPodeModificar(postagem)) return Unauthorized();

            await _postagemService.Remover(id);

            return RedirectToAction("Index");
        }



        private bool PostagemExists(Guid id)
        {
            return _postagemRepository.ObterPostagem(id) != null;
        }


        private async Task<PostagemViewModel?> ObterPostagem(Guid? id)
        {
            if (id == null)
                return null;

            var postagem = _mapper.Map<PostagemViewModel>(await _postagemRepository.ObterPostagem(Guid.Parse(id.ToString())));
            return postagem;
        }

        private bool UsuarioPodeModificar(PostagemViewModel comentario)
        {
            return UserAdmin || UserId == comentario.IdAutor;
        }

        private async Task CriarAutorSeNaoExistir()
        {
            IEnumerable<Autor> _autor = await _autorRepository.Buscar(p => p.Id == UserId);

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
        }

    }
}
