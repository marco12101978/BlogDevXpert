using AutoMapper;
using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postagem>>> GetPostagens()
        {
            //return await _context.Postagens.ToListAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Postagem>> GetPostagem(Guid id)
        {
            //var postagem = await _context.Postagens.FindAsync(id);

            //if (postagem == null)
            //{
            //    return NotFound();
            //}

            //return postagem;

            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostagem(Guid id, Postagem postagem)
        {
            //if (id != postagem.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(postagem).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PostagemExists(id))
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
        public async Task<ActionResult<Postagem>> PostPostagem(Postagem postagem)
        {
            //_context.Postagens.Add(postagem);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPostagem", new { id = postagem.Id }, postagem);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostagem(Guid id)
        {
            //var postagem = await _context.Postagens.FindAsync(id);
            //if (postagem == null)
            //{
            //    return NotFound();
            //}

            //_context.Postagens.Remove(postagem);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return Ok();
        }

        private bool PostagemExists(Guid id)
        {
            // return _context.Postagens.Any(e => e.Id == id);

            return false;
        }
    }
}
