using Blog.Business.Models.Validations.Documentos;
using FluentValidation;

namespace Blog.Business.Models.Validations
{
    public class PostagemValidation : AbstractValidator<Postagem>
    {
        public PostagemValidation()
        {

            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.DataCriacao)
                    .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                    .Must(ValidacaoDatetime.EhUmaDataValida).WithMessage("O campo {PropertyName} precisa ser uma data válida");

        }
    }
}
