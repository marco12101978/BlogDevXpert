﻿using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MeuDbContext context) : base(context)
        {
        }

        public async Task<bool> ExiteTabela()
        {
            try
            {
                var anyEntity = await Db.Comentarios.AnyAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
