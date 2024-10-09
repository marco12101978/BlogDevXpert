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
    public class PostagemRepository : Repository<Postagem>, IPostagemRepository
    {
        public PostagemRepository(MeuDbContext context) : base(context)
        {

        }

        public async Task<bool> ExiteTabela()
        {
            try
            {
                var anyEntity = await Db.Postagens.AnyAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Postagem?> ObterPostagem(Guid postagemId)
        {
            return await Db.Postagens.AsNoTracking()
                                     .Include(p => p.Autor)
                                     .Include(p => p.Comentarios)
                                     .FirstOrDefaultAsync(p => p.Id == postagemId);


        }

        public async Task<List<Postagem>> ObterTodasPostagem()
        {

            return await Db.Postagens.AsNoTracking()
                                     .Include(p => p.Autor)
                                     .OrderByDescending(p => p.DataCriacao)
                                     .ToListAsync();
        }

        public async Task<List<Postagem>> ObterTodasPostagemEComentarios()
        {
            return await Db.Postagens.AsNoTracking()
                         .Include(p => p.Autor)
                         .Include(p => p.Comentarios)
                         .OrderByDescending(p => p.DataCriacao)
                         .ToListAsync();
        }
    }
}
