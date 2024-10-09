using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    [Route("api/postagens")]
    [ApiController]
    public class PostagensController : ControllerBase
    {
        private readonly MeuDbContext _context;

        public PostagensController(MeuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postagem>>> GetPostagens()
        {
            return await _context.Postagens.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Postagem>> GetPostagem(Guid id)
        {
            var postagem = await _context.Postagens.FindAsync(id);

            if (postagem == null)
            {
                return NotFound();
            }

            return postagem;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostagem(Guid id, Postagem postagem)
        {
            if (id != postagem.Id)
            {
                return BadRequest();
            }

            _context.Entry(postagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostagemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Postagem>> PostPostagem(Postagem postagem)
        {
            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostagem", new { id = postagem.Id }, postagem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostagem(Guid id)
        {
            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem == null)
            {
                return NotFound();
            }

            _context.Postagens.Remove(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostagemExists(Guid id)
        {
            return _context.Postagens.Any(e => e.Id == id);
        }
    }
}
