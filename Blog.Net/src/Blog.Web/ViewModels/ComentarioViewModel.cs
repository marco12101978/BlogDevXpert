using Blog.Business.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels
{
    public class ComentarioViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(2000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "O campo {0} precisa ser uma data válida")]
        [Display(Name = "Data Postagem")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataPostagem { get; set; }

        public string? NomeAutor { get; set; }

        public Guid IdAutor { get; set; }

        public Guid IdPostagem { get; set; }

        public PostagemViewModel? Postagem { get; set; }

    }
}
