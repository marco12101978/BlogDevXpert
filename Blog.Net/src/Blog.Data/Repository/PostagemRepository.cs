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
    public class PostagemRepository : Repository<Postagem>, IPostagemRepository
    {
        public PostagemRepository(MeuDbContext context) : base(context)
        {

        }
    }
}
