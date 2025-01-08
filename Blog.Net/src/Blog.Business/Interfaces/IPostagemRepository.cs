using Blog.Business.Models;

namespace Blog.Business.Interfaces
{
    public interface IPostagemRepository : IRepository<Postagem>
    {
        Task<Postagem> ObterPostagem(Guid postagemId);

        Task<List<Postagem>> ObterTodasPostagem();

        Task<List<Postagem>> ObterTodasPostagemEComentarios();

        Task<bool> ExiteTabela();

    }
}
