using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Services
{
    public class PostagemService : BaseService, IPostagemService
    {
        private readonly IPostagemRepository _postagemrepository;

        public PostagemService(IPostagemRepository postagemrepository, INotificador notificador) : base(notificador)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;
            _postagemrepository = postagemrepository;
        }

        public Task Adicionar(Comentario comentario)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(Comentario comentario)
        {
            throw new NotImplementedException();
        }

        public Task Remover(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _postagemrepository.Dispose();
        }
    }
}
