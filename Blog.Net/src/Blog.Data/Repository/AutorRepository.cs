﻿using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MeuDbContext context) : base(context)
        {
        }

        public async Task<Autor> ObterPostagem(Guid id)
        {
            // return await Buscar(p => p.Id == id);

            return await Db.Autores.AsNoTracking().Include(f => f.Id).FirstOrDefaultAsync(p => p.Id == id);

        }
    }
}
