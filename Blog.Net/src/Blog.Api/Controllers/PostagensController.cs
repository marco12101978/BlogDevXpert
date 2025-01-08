using AutoMapper;
using Blog.Api.ViewModels.Postagem;
using Blog.Business.Interfaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if (!await VerificarTabelaPostagem()) return CustomResponse(HttpStatusCode.InternalServerError);

            var resultado = await _postagemRepository.ObterTodasPostagemEComentarios();


            return _mapper.Map<List<PostagemViewModel>>(resultado);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PostagemViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostagemViewModel>> ObterPorId(Guid id)
        {
            if (!await VerificarTabelaPostagem()) return CustomResponse(HttpStatusCode.InternalServerError);

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
            if (!await VerificarTabelaPostagem()) return CustomResponse(HttpStatusCode.InternalServerError);

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await CriarAutorSeNaoExistir();



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
                return CustomResponse(HttpStatusCode.BadRequest);
            }

            if (!await VerificarTabelaPostagem()) return CustomResponse(HttpStatusCode.InternalServerError);


            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var _postagemAtualizacao = await ObterPostagem(id);

            if (_postagemAtualizacao == null)
            {
                return NotFound();
            }

            if (UsuarioPodeModificar(_postagemAtualizacao))
            {
                _postagemAtualizacao.Titulo = postagemUpdateModel.Titulo;
                _postagemAtualizacao.Conteudo = postagemUpdateModel.Conteudo;
                _postagemAtualizacao.DataAtualizacao = DateTime.Now;

                await _postagemService.Atualizar(_mapper.Map<Postagem>(_postagemAtualizacao));

                return CustomResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Unauthorized();
            }
        }



        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            if (!await VerificarTabelaPostagem()) return CustomResponse(HttpStatusCode.InternalServerError);

            var postagem = await ObterPostagem(id);

            if (postagem == null) return NotFound();


            if (UsuarioPodeModificar(postagem))
            {
                await _postagemService.Remover(id);

                return CustomResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Unauthorized();
            }

        }


        private async Task<PostagemViewModel?> ObterPostagem(Guid? id)
        {
            if (id == null) return null;

            return _mapper.Map<PostagemViewModel>(await _postagemRepository.ObterPostagem(id ?? Guid.Empty));
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

        private async Task<bool> VerificarTabelaPostagem()
        {
            if (!await _postagemRepository.ExiteTabela())
            {
                NotificarErro("Falha ao processar dados. Favor entrar em contato com o suporte.");
                return false;
            }
            return true;
        }

        private bool UsuarioPodeModificar(PostagemViewModel comentario)
        {
            return UserAdmin || UserId == comentario.IdAutor;
        }
    }
}
