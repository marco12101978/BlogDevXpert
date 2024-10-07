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
        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }


        [Display(Name = "Data Postagem")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DataPostagem { get; set; }

        public string? NomeAutor { get; set; }

        public Guid IdAutor { get; set; }

        public Guid IdPostagem { get; set; }

        public Postagem? Postagem { get; set; }
    }
}
