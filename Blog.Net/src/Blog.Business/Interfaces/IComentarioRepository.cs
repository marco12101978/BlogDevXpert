using Blog.Business.Models;

namespace Blog.Business.Interfaces
{
    public interface IComentarioRepository : IRepository<Comentario>
    {
        Task<Comentario> ObterComentario(Guid postagemId);

        Task<List<Comentario>> ObterTodosComentarios();

        Task<bool> ExiteTabela();
    }
}
