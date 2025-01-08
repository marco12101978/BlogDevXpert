using Blog.Business.Models;

namespace Blog.Business.Interfaces
{
    public interface IPostagemService : IDisposable
    {
        Task Adicionar(Postagem comentario);
        Task Atualizar(Postagem comentario);
        Task Remover(Guid id);
    }
}
