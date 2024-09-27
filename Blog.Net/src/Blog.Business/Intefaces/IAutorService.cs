using Blog.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Intefaces
{
    public interface IAutorService : IDisposable
    {
        Task Adicionar(Autor autor);
        Task Atualizar(Autor autor);
        Task Remover(Guid id);
    }
}
