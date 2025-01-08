using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Business.Models.Validations;

namespace Blog.Business.Services
{
    public class PostagemService : BaseService, IPostagemService
    {
        private readonly IPostagemRepository _postagemrepository;

        public PostagemService(IPostagemRepository postagemrepository, INotificador notificador) : base(notificador)
        {

            _postagemrepository = postagemrepository;
        }

        public async Task Adicionar(Postagem postagem)
        {
            if (!ExecutarValidacao(new PostagemValidation(), postagem)) return;

            await _postagemrepository.Adicionar(postagem);
        }

        public async Task Atualizar(Postagem postagem)
        {
            if (!ExecutarValidacao(new PostagemValidation(), postagem)) return;

            await _postagemrepository.Atualizar(postagem);
        }

        public async Task Remover(Guid id)
        {
            var autor = await _postagemrepository.ObterPorId(id);

            if (autor == null)
            {
                Notificar("Postagem não existe!");
                return;
            }
            await _postagemrepository.Remover(id);
        }

        public void Dispose()
        {
            _postagemrepository?.Dispose();
        }
    }
}
