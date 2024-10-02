using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Models
{
    public class Comentario : Entity
    {
        public string? Conteudo { get; set; }

        public DateTime? DataPostagem { get; set; }

        public string? NomeAutor { get; set; }

        public Guid IdAutor { get; set; }

        public Guid IdPostagem { get; set; }

        public Postagem? Postagem { get; set; }
    }
}
