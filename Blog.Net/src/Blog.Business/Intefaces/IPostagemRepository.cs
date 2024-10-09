using Blog.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Intefaces
{
    public interface IPostagemRepository : IRepository<Postagem>
    {
        Task<Postagem> ObterPostagem(Guid postagemId);

        Task<List<Postagem>> ObterTodasPostagem();

        Task<List<Postagem>> ObterTodasPostagemEComentarios();

        Task<bool> ExiteTabela();

    }
}
