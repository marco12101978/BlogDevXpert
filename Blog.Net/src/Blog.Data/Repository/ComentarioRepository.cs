using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Data.Context;
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
    }
}
