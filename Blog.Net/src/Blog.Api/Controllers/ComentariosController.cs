using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Api.ViewModels.Comentario;
using Blog.Business.Intefaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.Api.Controllers
{
    [Authorize]
    [Route("api/comentarios")]
    [ApiController]
    public class ComentariosController : MainController
    {
        private readonly IMapper _mapper;

        private readonly IComentarioRepository _comentarioRepository;
        private readonly IComentarioService _comentarioService;

        private readonly IAutorRepository _autorRepository;
        private readonly IAutorService _autorService;

        public ComentariosController(IMapper mapper,
                                     IComentarioRepository repository,
                                     IComentarioService service,
                                     IAutorRepository autorRepository,
                                     IAutorService autorService,
                                     INotificador notificador,
                                     IAppIdentityUser user) : base(notificador, user)
        {
            _mapper = mapper;

            _comentarioRepository = repository;
            _comentarioService = service;

            _autorRepository = autorRepository;
            _autorService = autorService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<ComentarioViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ComentarioViewModel>>> ObterTodos()
        {
            if (!await _comentarioRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Obter dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return _mapper.Map<List<ComentarioViewModel>>(await _comentarioRepository.ObterTodosComentarios());
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ComentarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComentarioViewModel>> ObterPorId(Guid id)
        {
            if (!await _comentarioRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Obter dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var comentario = await ObterComentario(id);

            if (comentario == null)
            {
                return NotFound();
            }
            return comentario;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ComentarioViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComentarioViewModel>> Adicionar(ComentarioViewModel comentario)
        {
            if (!await _comentarioRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Inserir dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            await CriarUsuarioSemNaoExistir();


            if (!ModelState.IsValid) return CustomResponse(ModelState);


            ComentarioViewModel xnxx = new ComentarioViewModel();

            xnxx.Id = Guid.NewGuid();
            
            xnxx.Conteudo = comentario.Conteudo;

            xnxx.IdAutor = UserId;
            xnxx.NomeAutor = UserName;

            xnxx.IdPostagem = comentario.IdPostagem;
            xnxx.DataPostagem = DateTime.Now;




            await _comentarioRepository.Adicionar(_mapper.Map<Comentario>(comentario));

            return CustomResponse(HttpStatusCode.Created, comentario);

        }


        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Atualizar(Guid id, ComentarioViewModel comentario)
        {
            if (id != comentario.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse(HttpStatusCode.OK);
            }

            if (!await _comentarioRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Atualizar dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var comentarioAtualizacao = await ObterComentario(id);

            if (comentarioAtualizacao == null)
            {
                return NotFound();
            }

            if (UserAdmin || UserId == comentario.IdAutor)
            {
                comentarioAtualizacao.Conteudo = comentario.Conteudo;

                await _comentarioService.Atualizar(_mapper.Map<Comentario>(comentarioAtualizacao));

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
            if (!await _comentarioRepository.ExiteTabela())
            {
                NotificarErro("Falha ao Excluir dados favor entrar em contato com o responsavel técnico");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var comentario = await ObterComentario(id);

            if (comentario == null) return NotFound();

            if (UserAdmin || UserId == comentario.IdAutor)
            {
                await _comentarioService.Remover(id);

                return CustomResponse(HttpStatusCode.NoContent);
            }
            else
            {

                return Unauthorized();
            }
        }


        private async Task<ComentarioViewModel?> ObterComentario(Guid? id)
        {
            if (id == null) return null;

            ComentarioViewModel comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.ObterComentario(id ?? Guid.Empty));
            return comentario;
        }


        private async Task CriarUsuarioSemNaoExistir()
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
