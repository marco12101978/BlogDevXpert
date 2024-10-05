using System.ComponentModel.DataAnnotations;

namespace Blog.Business.Models
{
    public class Postagem : Entity
    {

        public string? Titulo { get; set; }

        public string? Conteudo { get; set; }

        [Display(Name = "Data Postagem")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataCriacao { get; set; }

        [Display(Name = "Data Atualização")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataAtualizacao { get; set; }

        public Guid IdAutor { get; set; }
        public Autor? Autor { get; set; }

        public List<Comentario>? Comentarios { get; set; }

    }
}
