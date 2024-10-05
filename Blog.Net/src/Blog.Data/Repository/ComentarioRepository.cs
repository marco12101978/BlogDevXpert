using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MeuDbContext context) : base(context)
        {
        }

        public async Task<Comentario?> ObterComentario(Guid postagemId)
        {
            return await Db.Comentarios.AsNoTracking()
                          .Include(c => c.Postagem)
                          .FirstOrDefaultAsync(p => p.Id == postagemId);
        }

        public async Task<List<Comentario>> ObterTodosComentarios()
        {
            return await Db.Comentarios.AsNoTracking()
                          .Include(c => c.Postagem)
                          .OrderByDescending(p => p.DataPostagem)
                          .ToListAsync();

        }
    }
}
