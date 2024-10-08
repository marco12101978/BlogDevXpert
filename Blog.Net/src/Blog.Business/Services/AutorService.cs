using Blog.Business.Intefaces;
using Blog.Business.Models;
using Blog.Business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Services
{
    public class AutorService : BaseService, IAutorService
    {
        private readonly IAutorRepository _autorRepository;
        public AutorService(IAutorRepository repository, INotificador notificador) : base(notificador)
        {
            _autorRepository = repository;
        }

        public async Task Adicionar(Autor autor)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;

            if (_autorRepository.Buscar(f => f.Email == autor.Email).Result.Any())
            {
                Notificar("Já existe um autor com este email informado.");
                return;
            }

            await _autorRepository.Adicionar(autor);

        }

        public async Task Atualizar(Autor autor)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;

            if (_autorRepository.Buscar(f => f.Email == autor.Email && f.Id != autor.Id).Result.Any())
            {
                Notificar("Já existe um autor com este email informado.");
                return;
            }

            await _autorRepository.Atualizar(autor);
        }


        public async Task Remover(Guid id)
        {
            var autor = await _autorRepository.ObterPorId(id);

            if (autor == null)
            {
                Notificar("Autor não existe!");
                return;
            }

            await _autorRepository.Remover(id);
        }

        public void Dispose()
        {
            _autorRepository?.Dispose();
        }
    }
}
