using Blog.Business.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels
{
    public class PostagemViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(2000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }

        [Display(Name = "Data Postagem")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataCriacao { get; set; }

        [Display(Name = "Data Atualização")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataAtualizacao { get; set; }

        public Guid IdAutor { get; set; }
        public Autor? Autor { get; set; }

        public List<ComentarioViewModel>? Comentarios { get; set; }
    }
}
