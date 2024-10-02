using Blog.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Intefaces
{
    public interface IPostagemService : IDisposable
    {
        Task Adicionar(Postagem comentario);
        Task Atualizar(Postagem comentario);
        Task Remover(Guid id);
    }
}
