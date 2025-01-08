using Blog.Business.Models;

namespace Blog.Business.Interfaces
{
    public interface IComentarioService : IDisposable
    {
        Task Adicionar(Comentario comentario);
        Task Atualizar(Comentario comentario);
        Task Remover(Guid id);
    }
}
