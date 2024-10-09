﻿using Blog.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Intefaces
{
    public interface IComentarioRepository : IRepository<Comentario>
    {
        Task<Comentario> ObterComentario(Guid postagemId);

        Task<List<Comentario>> ObterTodosComentarios();

        Task<bool> ExiteTabela();
    }
}
