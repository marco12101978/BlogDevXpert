using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels.Comentario
{

    public class ComentarioInputModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório e deve ser um GUID válido.")]
        public Guid IdPostagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }

    }
}
