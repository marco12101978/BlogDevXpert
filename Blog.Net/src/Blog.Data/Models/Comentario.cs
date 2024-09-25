using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Models
{
    public class Comentario
    {
        [Key]
        public Guid IdComentario { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Conteudo { get; set; }

        [Required]
        public DateTime DataPostagem { get; set; }

        [Required]
        [MaxLength(100)]
        public string? NomeAutor { get; set; }

        [ForeignKey("Postagem")]
        public Guid IdPostagem { get; set; }

        public Postagem? Postagem { get; set; }
    }
}
