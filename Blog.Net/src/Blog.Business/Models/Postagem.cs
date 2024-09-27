using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Models
{
    public class Postagem : Entity
    {

        public string? Titulo { get; set; }

        public string? Conteudo { get; set; }

        public DateTime? DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public Guid IdAutor { get; set; }
        public Autor? Autor { get; set; }

        public List<Comentario>? Comentarios { get; set; }

    }
}
