using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;

namespace Blog.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MeuDbContext context) : base(context)
        {
        }

    }
}
