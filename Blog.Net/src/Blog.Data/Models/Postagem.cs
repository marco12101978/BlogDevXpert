using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class Postagem
    {
        [Key]
        public Guid IdPostagem { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Conteudo { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        [ForeignKey("Autor")]
        public Guid IdAutor { get; set; }
        public Autor Autor { get; set; }

        public List<Comentario> Comentarios { get; set; }

        public List<string> Tags { get; set; }
    }
}
