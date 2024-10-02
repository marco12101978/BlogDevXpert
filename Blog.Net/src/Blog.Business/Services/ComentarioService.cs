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
    public class ComentarioService : BaseService, IComentarioService
    {
        private readonly IComentarioRepository _comentarioRepository;
        public ComentarioService(IComentarioRepository comentarioRepository, INotificador notificador) : base(notificador)
        {
            _comentarioRepository = comentarioRepository;
        }
        public async Task Adicionar(Comentario comentario)
        {
            if (!ExecutarValidacao(new ComentarioValidation(), comentario)) return;

            await _comentarioRepository.Adicionar(comentario);
        }

        public async Task Atualizar(Comentario comentario)
        {
            if (!ExecutarValidacao(new ComentarioValidation(), comentario)) return;

            await _comentarioRepository.Atualizar(comentario);

        }


        public async Task Remover(Guid id)
        {
            var autor = await _comentarioRepository.ObterPorId(id);

            if (autor == null)
            {
                Notificar("Postagem não existe!");
                return;
            }

            await _comentarioRepository.Remover(id);
        }

        public void Dispose()
        {
            _comentarioRepository?.Dispose();
        }
    }
}
