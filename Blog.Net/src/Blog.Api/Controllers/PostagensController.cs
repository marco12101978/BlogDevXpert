using AutoMapper;
using Blog.Api.ViewModels.Postagem;
using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Business.Services;
using Blog.Data.Context;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Blog.Api.Controllers
{
    [Authorize]
    [Route("api/postagens")]
    [ApiController]
    public class PostagensController : MainController
    {
        private readonly IMapper _mapper;

        private readonly IPostagemRepository _postagemRepository;
        private readonly IPostagemService _postagemService;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;

        public PostagensController(IMapper mapper,
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
        [ProducesResponseType(typeof(List<PostagemViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PostagemViewModel>>> ObterTodos()
        {
            if (!await _postagemRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Obter dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var resultado = await _postagemRepository.ObterTodasPostagemEComentarios();


            return _mapper.Map<List<PostagemViewModel>>(resultado);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PostagemViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostagemViewModel>> ObterPorId(Guid id)
        {
            if (!await _postagemRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Obter dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var comentario = await ObterPostagem(id);

            if (comentario == null)
            {
                return NotFound();
            }
            return comentario;
        }


        [HttpPost]
        [ProducesResponseType(typeof(PostagemViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostagemViewModel>> Adicionar(PostagemInputModel postagemInputModel)
        {
            if (!await _postagemRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Inserir dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await CriarUsuarioCasoNaoExistir();

            PostagemViewModel _postagem = new PostagemViewModel
            {
                DataCriacao = DateTime.Now,
                Titulo = postagemInputModel.Titulo,
                Conteudo = postagemInputModel.Conteudo,
                IdAutor = UserId,
                Id = Guid.NewGuid()
            };

            await _postagemService.Adicionar(_mapper.Map<Postagem>(_postagem));



            return CustomResponse(HttpStatusCode.Created, _postagem);

        }


        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Atualizar(Guid id, PostagemUpdateModel postagemUpdateModel)
        {

            if (id != postagemUpdateModel.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse();
            }

            if (!await _postagemRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Atualizar dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }


            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var _postagemAtualizacao = await ObterPostagem(id);

            if (_postagemAtualizacao == null)
            {
                return NotFound();
            }

            if (UserAdmin || UserId == _postagemAtualizacao.IdAutor)
            {
                _postagemAtualizacao.Titulo = postagemUpdateModel.Titulo;
                _postagemAtualizacao.Conteudo = postagemUpdateModel.Conteudo;
                _postagemAtualizacao.DataAtualizacao = DateTime.Now;

                await _postagemService.Atualizar(_mapper.Map<Postagem>(_postagemAtualizacao));

                return CustomResponse(HttpStatusCode.NoContent);
            }
            else
            {
                NotificarErro("Falha ao Excluir , sem autorização");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

        }



        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            if (!await _postagemRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Excluir dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var comentario = await ObterPostagem(id);

            if (comentario == null) return NotFound();


            if (UserAdmin || UserId == comentario.IdAutor)
            {
                await _postagemService.Remover(id);

                return CustomResponse(HttpStatusCode.NoContent);
            }
            else
            {
                NotificarErro("Falha ao Excluir , sem autorização ");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

        }


        private async Task<PostagemViewModel?> ObterPostagem(Guid? id)
        {
            if (id == null) return null;

            PostagemViewModel comentario = _mapper.Map<PostagemViewModel>(await _postagemRepository.ObterPostagem(id ?? Guid.Empty));
            return comentario;
        }


        private async Task CriarUsuarioCasoNaoExistir()
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
