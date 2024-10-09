using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
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

        [HttpGet]
        public async Task<IEnumerable<ComentarioViewModel>> GetComentarios()
        {
            var xx = UserId;
            var xxx = UserAdmin;

            return _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterTodos());

            //return await _context.Comentarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetComentario(Guid id)
        {
            //var comentario = await _context.Comentarios.FindAsync(id);

            //if (comentario == null)
            //{
            //    return NotFound();
            //}

            //return comentario;
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentario(Guid id, Comentario comentario)
        {
            //if (id != comentario.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(comentario).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ExisteComentario(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Comentario>> PostComentario(Comentario comentario)
        {
            //_context.Comentarios.Add(comentario);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetComentario", new { id = comentario.Id }, comentario);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(Guid id)
        {
            //var comentario = await _context.Comentarios.FindAsync(id);
            //if (comentario == null)
            //{
            //    return NotFound();
            //}

            //_context.Comentarios.Remove(comentario);
            //await _context.SaveChangesAsync();

            //return NoContent();

            return Ok();
        }


        private bool ExisteComentario(Guid id)
        {
            return _comentarioRepository.ObterPorId(id) != null ? true : false;
        }

        private async Task<ComentarioViewModel?> ObterComentario(Guid? id)
        {
            if (id == null)
                return null;

            ComentarioViewModel comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.ObterComentario(id ?? Guid.Empty));
            return comentario;
        }
    }
}
