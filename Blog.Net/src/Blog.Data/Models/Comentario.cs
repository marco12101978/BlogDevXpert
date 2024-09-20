using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class Comentario
    {
        [Key]
        public int IdComentario { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Conteudo { get; set; }

        [Required]
        public DateTime DataPostagem { get; set; }

        [Required]
        [MaxLength(100)]
        public string NomeAutor { get; set; }

        [ForeignKey("Postagem")]
        public int IdPostagem { get; set; }

        public Postagem Postagem { get; set; }
    }
}
