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


        public async Task<Postagem> ObterPostagem(Guid postagemId)
        {
            var XX = Db.Postagens.ToList();

            postagemId = new Guid("823f6873-6fce-49f0-a39b-9c58a831291f");

            return await Db.Postagens.AsNoTracking().Include(p => p.Autor).FirstOrDefaultAsync(p => p.Id == postagemId);



        }
    }
}
