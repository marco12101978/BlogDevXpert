using Blog.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Intefaces
{
    public interface IComentarioService : IDisposable
    {
        Task Adicionar(Comentario comentario);
        Task Atualizar(Comentario comentario);
        Task Remover(Guid id);
    }
}
