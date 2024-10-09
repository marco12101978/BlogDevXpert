using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    [Route("api/comentarios")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly MeuDbContext _context;

        public ComentariosController(MeuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentarios()
        {
            return await _context.Comentarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetComentario(Guid id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return comentario;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentario(Guid id, Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return BadRequest();
            }

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
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
        public async Task<ActionResult<Comentario>> PostComentario(Comentario comentario)
        {
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComentario", new { id = comentario.Id }, comentario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(Guid id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioExists(Guid id)
        {
            return _context.Comentarios.Any(e => e.Id == id);
        }
    }
}
