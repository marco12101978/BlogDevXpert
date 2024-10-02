using Blog.Business.Models.Validations.Documentos;
using FluentValidation;

namespace Blog.Business.Models.Validations
{
    public class ComentarioValidation : AbstractValidator<Comentario>
    {
        public ComentarioValidation()
        {
            RuleFor(c => c.Conteudo)
                    .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                    .Length(10, 1000).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.DataPostagem)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Must(ValidacaoDatetime.EhUmaDataValida).WithMessage("O campo {PropertyName} precisa ser uma data válida");

            RuleFor(c => c.NomeAutor)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(3, 100).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.IdAutor)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} precisa ser um GUID válido");

            RuleFor(c => c.IdPostagem)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} precisa ser um GUID válido");
        }
    }
}
